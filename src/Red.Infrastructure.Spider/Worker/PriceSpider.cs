using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application;
using Red.Core.Application.Extensions;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider.Worker
{
    public class PriceSpider : TimedWorker
    {
        private readonly IEshop _eshop;
        private readonly IServiceProvider _serviceProvider;

        public PriceSpider(IAppLogger<PriceSpider> log,
                           IEshop eshop,
                           IServiceProvider serviceProvider)
            : base(log)
        {
            _eshop = eshop;
            _serviceProvider = serviceProvider;
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
            var repo = _serviceProvider.GetRequiredService<ISwitchGameRepository>();
            var games = await repo.Get()
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
            var chunks = games.Where(x => x.Nsuids.Count == 1).ChunkBy(50).ToList();
            var processChunksAtOnce = 10;

            foreach (var groupOfChunks in chunks.ChunkBy(processChunksAtOnce))
            {
                var tasks = groupOfChunks.Select(x => UpdateGames(x.ToList()));

                await Task.WhenAll(tasks);
            }
        }

        private async Task UpdateGames(IReadOnlyCollection<SwitchGame> games)
        {
            var repo = _serviceProvider.GetRequiredService<ISwitchGameRepository>();
            var nsuids = games.Select(x => x.Nsuids[0]);
            var query = new EshopMultiPriceQuery(nsuids);
            var prices = await _eshop.GetPrices(query);

            foreach (var price in prices)
            {
                // todo: cant use single when we allow games with multiple nsuids - see above
                var game = games.Single(x => x.Nsuids.Contains(price.Nsuid));
                await UpdatePrice(game, price, repo);
            }
        }

        private async Task UpdatePrice(SwitchGame game, SwitchGamePrice price, ISwitchGameRepository repo)
        {
            // todo: use actual country
            string country = "DE";
            var entity = await repo.GetByProductCode(game.ProductCode);
            var lastPrice = game.Price.History.OrderBy(x => x.Date).LastOrDefault(x => x.Country == country);
            var ctx = new UpdateContext
            {
                Game = game,
                Price = price,
                Country = country,
                LastPrice = lastPrice
            };

            var updatedPrice = entity.Price with { };
            updatedPrice = UpdateRegularPrice(ctx, updatedPrice);
            updatedPrice = UpdateDiscount(ctx, updatedPrice);
            updatedPrice = InitializeHistory(ctx, updatedPrice);
            updatedPrice = UpdateSalesStatus(ctx, updatedPrice);
            updatedPrice = UpdateAllTimeLow(ctx, updatedPrice);
            updatedPrice = UpdateAllTimeHigh(ctx, updatedPrice);

            var updatedEntity = entity with {Price = updatedPrice};
            if (!entity.Price.Equals(updatedEntity.Price))
            {
                Log.LogInformation("Update price for {title} ({productCode})", game.Title ?? "", game.ProductCode);
                await repo.UpdateAsync(updatedEntity);
            }
        }

        #region UpdateContext Methods

        private sealed record UpdateContext
        {
            public string Country { get; init; }
            public SwitchGame Game { get; init; }
            public DatedPriceRecord? LastPrice { get; init; }
            public SwitchGamePrice Price { get; init; }
        }

        private static SwitchGamePriceDetails InitializeHistory(UpdateContext ctx, SwitchGamePriceDetails details)
        {
            if (!ctx.Price.Discounted && !string.IsNullOrWhiteSpace(ctx.Price.Currency))
            {
                if (ctx.LastPrice == null || ctx.Price.RegularPrice.HasValue
                    && ctx.LastPrice.Price.Amount - ctx.Price.RegularPrice < 0.01)
                {
                    var history = ctx.Game.Price.History.ToList();
                    history.Add(DatedPriceRecord.New(ctx.Country, ctx.Price.RegularPrice!.Value, ctx.Price.Currency!));

                    return details with
                    {
                        History = history
                    };
                }
            }

            return details;
        }

        private static SwitchGamePriceDetails UpdateAllTimeHigh(UpdateContext ctx, SwitchGamePriceDetails details)
        {
            if (!ctx.Price.RegularPrice.HasValue || string.IsNullOrWhiteSpace(ctx.Price.Currency))
            {
                return details;
            }

            var ath = details.AllTimeHigh.SingleOrDefault(x => x.Country == ctx.Country);
            var highestPrice = Math.Max(ctx.Price.CurrentPrice ?? int.MinValue, ctx.Price.RegularPrice.Value);

            if (ath == null || ath.Price.Amount < highestPrice)
            {
                var newAtl = details.AllTimeHigh.ToList();
                newAtl.Add(UndatedPriceRecord.New(ctx.Country, highestPrice, ctx.Price.Currency));

                return details with
                {
                    AllTimeHigh = newAtl
                };
            }

            return details;
        }

        private static SwitchGamePriceDetails UpdateAllTimeLow(UpdateContext ctx, SwitchGamePriceDetails details)
        {
            if (!ctx.Price.RegularPrice.HasValue || string.IsNullOrWhiteSpace(ctx.Price.Currency))
            {
                return details;
            }

            var atl = details.AllTimeLow.SingleOrDefault(x => x.Country == ctx.Country);
            var lowestPrice = Math.Min(ctx.Price.CurrentPrice ?? int.MaxValue, ctx.Price.RegularPrice.Value);

            if (atl == null || atl.Price.Amount > lowestPrice)
            {
                var newAtl = details.AllTimeLow.ToList();
                newAtl.Add(UndatedPriceRecord.New(ctx.Country, lowestPrice, ctx.Price.Currency));

                return details with
                {
                    AllTimeLow = newAtl
                };
            }

            return details;
        }

        private static SwitchGamePriceDetails UpdateDiscount(UpdateContext ctx, SwitchGamePriceDetails details)
        {
            if (ctx.Price.Discounted && ctx.Price.CurrentPrice.HasValue && !string.IsNullOrWhiteSpace(ctx.Price.Currency))
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (ctx.LastPrice == null || ctx.LastPrice.Price.Amount != ctx.Price.CurrentPrice)
                {
                    var history = ctx.Game.Price.History.ToList();
                    history.Add(DatedPriceRecord.New(ctx.Country, ctx.Price.CurrentPrice!.Value, ctx.Price.Currency!));

                    return details with
                    {
                        History = history
                    };
                }
            }

            return details;
        }

        private static SwitchGamePriceDetails UpdateRegularPrice(UpdateContext ctx, SwitchGamePriceDetails details)
        {
            if (ctx.Price.RegularPrice.HasValue && !string.IsNullOrWhiteSpace(ctx.Price.Currency))
            {
                var rp = ctx.Game.Price.RegularPrice;
                rp[ctx.Country] = UndatedPriceRecord.New(ctx.Country, ctx.Price.RegularPrice.Value, ctx.Price.Currency);

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

        private static SwitchGamePriceDetails UpdateSalesStatus(UpdateContext ctx, SwitchGamePriceDetails details)
        {
            if (ctx.Price.SalesStatus != details.SalesStatus)
            {
                return details with
                {
                    SalesStatus = ctx.Price.SalesStatus
                };
            }

            return details;
        }

        #endregion
    }
}