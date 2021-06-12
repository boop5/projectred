using System;
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
                                   .Select(
                                       x => new SwitchGame
                                       {
                                           Nsuids = x.Nsuids,
                                           ProductCode = x.ProductCode,
                                           Region = x.Region,
                                           Price = x.Price
                                       })
                                   .ToListAsync(stoppingToken);

            var nsuids = games.Where(x => x.Nsuids.Count == 1).Select(x => x.Nsuids[0]);

            foreach (var chunk in nsuids.ChunkBy(50))
            {
                // todo: use actual country
                var country = "DE";
                var query = new EshopMultiPriceQuery(chunk);
                var prices = await _eshop.GetPrices(query);

                foreach (var price in prices)
                {
                    var game = games.Single(x => x.Nsuids.Contains(price.Nsuid));
                    var entity = await _repo.GetByProductCode(game.ProductCode);
                    var updatedEntity = entity with { };

                    if (price.RegularPrice.HasValue && !string.IsNullOrWhiteSpace(price.Currency))
                    {
                        var rp = game.Price.RegularPrice;
                        rp[country] = new UndatedPriceRecord
                        {
                            Price = new Price()
                            {
                                Amount = price.RegularPrice.Value,
                                Currency = price.Currency
                            },
                            Country = country
                        };

                        if (!updatedEntity.Price.RegularPrice.Equals(rp))
                        {
                            updatedEntity = updatedEntity with
                            {
                                Price = entity.Price with
                                {
                                    RegularPrice = rp
                                }
                            };
                        }
                    }
                    var lod = game.Price.History.OrderBy(x => x.Date)
                                  .LastOrDefault(x => x.Country == country);
                    if (price.Discounted && price.CurrentPrice.HasValue && !string.IsNullOrWhiteSpace(price.Currency))
                    {
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        if (lod == null || lod.Price.Amount != price.CurrentPrice)
                        {
                            var history = game.Price.History.ToList();
                            history.Add(DatedPriceRecord.New(country, price.CurrentPrice!.Value, price.Currency!));
                            
                            updatedEntity = updatedEntity with
                            {
                                Price = updatedEntity.Price with
                                {
                                    History = history
                                }
                            };
                        }
                    }

                    if (!price.Discounted && !string.IsNullOrWhiteSpace(price.Currency))
                    {
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        if(lod == null || (price.RegularPrice.HasValue && lod.Price.Amount != price.RegularPrice))
                        {
                            var history = game.Price.History.ToList();
                            history.Add(DatedPriceRecord.New(country, price.RegularPrice!.Value, price.Currency!));
                            
                            updatedEntity = updatedEntity with
                            {
                                Price = updatedEntity.Price with
                                {
                                    History = history
                                }
                            };
                        }
                    }

                    if (price.SalesStatus != updatedEntity.Price.SalesStatus)
                    {
                        updatedEntity = updatedEntity with
                        {
                            Price = updatedEntity.Price with
                            {
                                SalesStatus = price.SalesStatus ?? EshopSalesStatus.Unknown
                            }
                        };
                    }

                    if (!entity.Price.Equals(updatedEntity.Price))
                    {
                        await _repo.UpdateAsync(updatedEntity);
                    }
                }
            }
        }
    }
}