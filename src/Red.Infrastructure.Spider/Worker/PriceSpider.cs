using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Red.Core.Application;
using Red.Core.Application.Extensions;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider.Worker
{
    public class PriceSpider : TimedWorker
    {
        private readonly IEshop _eshop;
        private readonly ISwitchGameRepository _repo;

        public PriceSpider(IAppLogger<PriceSpider> log,
                           IEshop eshop,
                           ISwitchGameRepository repo)
            : base(log)
        {
            _eshop = eshop;
            _repo = repo;
        }

        protected override TimeSpan GetInitialDelay()
        {
            return TimeSpan.FromMinutes(0);
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            var games = await _repo.Get()
                                   .OrderByDescending(x => x.ReleaseDate)
                                   .Select(
                                       x => new SwitchGame
                                       {
                                           Nsuids = x.Nsuids,
                                           ProductCode = x.ProductCode,
                                           Region = x.Region,
                                           Price = x.Price,
                                           Title = x.Title
                                       })
                                   .ToListAsync(stoppingToken);

            // todo: what are we gonna do about games with multiple nsuids?
            foreach (var chunk in games.Where(x => x.Nsuids.Count == 1).ChunkBy(50))
            {
                await UpdateGames(chunk.ToList());
            }
        }

        private async Task UpdateGames(IReadOnlyCollection<SwitchGame> games)
        {
            var nsuids = games.Select(x => x.Nsuids[0]);
            var query = new EshopMultiPriceQuery(nsuids);
            var prices = await _eshop.GetPrices(query);

            foreach (var price in prices)
            {
                // todo: cant use single when we allow games with multiple nsuids - see above
                var game = games.Single(x => x.Nsuids.Contains(price.Nsuid));

                await UpdatePrice(game, price);
            }
        }

        private async Task UpdatePrice(SwitchGame game, SwitchGamePrice price)
        {
            // todo: use actual country
            var country = "DE";
            var entity = await _repo.GetByProductCode(game.ProductCode);
            var lastPrice = game.Price.History.OrderBy(x => x.Date).LastOrDefault(x => x.Country == country);
            
            // todo: refactor this into non-local methods
            #region Update Methods

            SwitchGamePriceDetails UpdateRegularPrice(SwitchGamePriceDetails details)
            {
                if (price.RegularPrice.HasValue && !string.IsNullOrWhiteSpace(price.Currency))
                {
                    var rp = game.Price.RegularPrice;
                    rp[country] = UndatedPriceRecord.New(country, price.RegularPrice.Value, price.Currency);

                    if (!details.RegularPrice.Equals(rp))
                    {
                        return details with
                        {
                            RegularPrice = rp
                        };
                    }
                }

                return details;
            }
            
            SwitchGamePriceDetails UpdateDiscount(SwitchGamePriceDetails details)
            {
                if (price.Discounted && price.CurrentPrice.HasValue && !string.IsNullOrWhiteSpace(price.Currency))
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (lastPrice == null || lastPrice.Price.Amount != price.CurrentPrice)
                    {
                        var history = game.Price.History.ToList();
                        history.Add(DatedPriceRecord.New(country, price.CurrentPrice!.Value, price.Currency!));

                        return details with
                        {
                            History = history
                        };
                    }
                }

                return details;
            }
           
            SwitchGamePriceDetails InitializeHistory(SwitchGamePriceDetails details)
            {
                if (!price.Discounted && !string.IsNullOrWhiteSpace(price.Currency))
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (lastPrice == null || price.RegularPrice.HasValue && lastPrice.Price.Amount != price.RegularPrice)
                    {
                        var history = game.Price.History.ToList();
                        history.Add(DatedPriceRecord.New(country!, price.RegularPrice!.Value, price.Currency!));

                        return details with
                        {
                            History = history
                        };
                    }
                }

                return details;
            }
           
            SwitchGamePriceDetails UpdateSalesStatus(SwitchGamePriceDetails details)
            {
                if (price.SalesStatus != details.SalesStatus)
                {
                    return details with
                    {
                        SalesStatus = price.SalesStatus
                    };
                }

                return details;
            }

            SwitchGamePriceDetails UpdateAllTimeLow(SwitchGamePriceDetails details)
            {
                if (!price.RegularPrice.HasValue || string.IsNullOrWhiteSpace(price.Currency))
                {
                    return details;
                }

                var atl = details.AllTimeLow.SingleOrDefault(x => x.Country == country);
                var lowestPrice = Math.Min(price.CurrentPrice ?? int.MaxValue, price.RegularPrice.Value);

                if (atl == null || atl.Price.Amount > lowestPrice)
                {
                    var newAtl = details.AllTimeLow.ToList();
                    newAtl.Add(UndatedPriceRecord.New(country, lowestPrice, price.Currency));

                    return details with
                    {
                        AllTimeLow = newAtl
                    };
                }

                return details;
            }

            SwitchGamePriceDetails UpdateAllTimeHigh(SwitchGamePriceDetails details)
            {
                if (!price.RegularPrice.HasValue || string.IsNullOrWhiteSpace(price.Currency))
                {
                    return details;
                }

                var ath = details.AllTimeHigh.SingleOrDefault(x => x.Country == country);
                var highestPrice = Math.Max(price.CurrentPrice ?? int.MinValue, price.RegularPrice.Value);

                if (ath == null || ath.Price.Amount < highestPrice)
                {
                    var newAtl = details.AllTimeHigh.ToList();
                    newAtl.Add(UndatedPriceRecord.New(country, highestPrice, price.Currency));

                    return details with
                    {
                        AllTimeHigh = newAtl
                    };
                }

                return details;
            }
            
            #endregion

            var updatedPrice = UpdateRegularPrice(entity.Price with { });
            updatedPrice = UpdateDiscount(updatedPrice);
            updatedPrice = InitializeHistory(updatedPrice);
            updatedPrice = UpdateSalesStatus(updatedPrice);
            updatedPrice = UpdateAllTimeLow(updatedPrice);
            updatedPrice = UpdateAllTimeHigh(updatedPrice);

            var updatedEntity = entity with {Price = updatedPrice};
            if (!entity.Price.Equals(updatedEntity.Price))
            {
                Log.LogInformation("Update price for {title} ({productCode})", game.Title ?? "", game.ProductCode);
                await _repo.UpdateAsync(updatedEntity);
            }
        }
    }
}