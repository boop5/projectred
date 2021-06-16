using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Extensions;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider.Worker
{
    internal sealed class PriceSpider : Spider
    {
        private readonly IEshop _eshop;
        private readonly IServiceProvider _serviceProvider;

        public PriceSpider(IAppLogger<PriceSpider> log,
                           PriceSpiderConfiguration configuration,
                           IEshop eshop,
                           IServiceProvider serviceProvider)
            : base(log, configuration)
        {
            _eshop = eshop;
            _serviceProvider = serviceProvider;
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            // todo: use proper country/locale
            var culture = new CultureInfo("en-DE");
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
                var tasks = groupOfChunks.Select(x => UpdateGames(culture, x.ToList()));

                await Task.WhenAll(tasks);
            }
        }

        private async Task UpdateGames(CultureInfo culture, IReadOnlyCollection<SwitchGame> games)
        {
            var repo = _serviceProvider.GetRequiredService<ISwitchGameRepository>();
            var nsuids = games.Select(x => x.Nsuids[0]);
            var query = new EshopMultiPriceQuery(culture, nsuids);
            var prices = await _eshop.GetPrices(query);

            foreach (var price in prices)
            {
                // todo: cant use single when we allow games with multiple nsuids - see above
                var game = games.Single(x => x.Nsuids.Contains(price.Nsuid));
                await UpdatePrice(culture, game, price, repo);
            }
        }

        private async Task UpdatePrice(CultureInfo culture, SwitchGame game, SwitchGamePrice price, ISwitchGameRepository repo)
        {
            var entity = (await repo.GetByFsId(game.FsId!))!;
            var country = culture.GetTwoLetterISORegionName();
            var lastPrice = game.Price.History[country]?.OrderBy(x => x.Date).LastOrDefault();
            var ctx = UpdateContext.New(culture, game, lastPrice, price);

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
            public static UpdateContext New(CultureInfo culture, SwitchGame game, DatedPrice? lastPrice, SwitchGamePrice price)
            {
                return new()
                {
                    Culture = culture ?? throw new Exception(),
                    Game = game ?? throw new Exception(),
                    LastPrice = lastPrice,
                    Price = price ?? throw new Exception()
                };
            }

            #pragma warning disable CS8618
            private UpdateContext()
            {
            }

            public CultureInfo Culture { get; private init; }
            public SwitchGame Game { get; private init; }
            public DatedPrice? LastPrice { get; private init; }
            public SwitchGamePrice Price { get; private init; }
            #pragma warning restore CS8618
        }

        private static SwitchGamePriceDetails InitializeHistory(UpdateContext ctx, SwitchGamePriceDetails details)
        {
            if (!ctx.Price.Discounted && !string.IsNullOrWhiteSpace(ctx.Price.Currency))
            {
                if (ctx.LastPrice == null || ctx.Price.RegularPrice.HasValue
                    && ctx.LastPrice.Amount - ctx.Price.RegularPrice < 0.01)
                {
                    var country = ctx.Culture.GetTwoLetterISORegionName();
                    var history = details.History with { };
                    var localHistory = history[country] ?? new List<DatedPrice>();
                    localHistory.Add(DatedPrice.New(ctx.Price.RegularPrice!.Value, ctx.Price.Currency!));
                    history[country] = localHistory.Distinct().ToList();

                    return details with {History = history};
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

            var country = ctx.Culture.GetTwoLetterISORegionName();
            var ath = details.AllTimeHigh with { };
            var localAth = ath[country];
            var highestPrice = Math.Max(ctx.Price.CurrentPrice ?? int.MinValue, ctx.Price.RegularPrice.Value);

            if (localAth == null || Math.Abs(localAth.Amount - highestPrice) < 0.01)
            {
                ath[country] = Price.New(highestPrice, ctx.Price.Currency);

                return details with {AllTimeHigh = ath};
            }

            return details;
        }

        private static SwitchGamePriceDetails UpdateAllTimeLow(UpdateContext ctx, SwitchGamePriceDetails details)
        {
            if (!ctx.Price.RegularPrice.HasValue || string.IsNullOrWhiteSpace(ctx.Price.Currency))
            {
                return details;
            }

            var country = ctx.Culture.GetTwoLetterISORegionName();
            var atl = details.AllTimeLow with { };
            var localAtl = atl[country];
            var lowestPrice = Math.Min(ctx.Price.CurrentPrice ?? int.MaxValue, ctx.Price.RegularPrice.Value);

            if (localAtl == null || Math.Abs(localAtl.Amount - lowestPrice) >= 0.01)
            {
                atl[country] = Price.New(lowestPrice, ctx.Price.Currency);

                return details with {AllTimeHigh = atl};
            }

            return details;
        }

        private static SwitchGamePriceDetails UpdateDiscount(UpdateContext ctx, SwitchGamePriceDetails details)
        {
            details = details with {OnDiscount = ctx.Price.Discounted};

            if (ctx.Price.Discounted && ctx.Price.CurrentPrice.HasValue && !string.IsNullOrWhiteSpace(ctx.Price.Currency))
            {
                if (ctx.LastPrice == null || Math.Abs(ctx.LastPrice.Amount - (float) ctx.Price.CurrentPrice) >= 0.01)
                {
                    var country = ctx.Culture.GetTwoLetterISORegionName();
                    var history = ctx.Game.Price.History with { };
                    history[country] ??= new List<DatedPrice>();
                    history[country]!.Add(DatedPrice.New(ctx.Price.CurrentPrice!.Value, ctx.Price.Currency));

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
                var country = ctx.Culture.GetTwoLetterISORegionName();
                var rp = ctx.Game.Price.RegularPrice with { };
                rp[country] = Price.New(ctx.Price.RegularPrice.Value, ctx.Price.Currency);

                if (details.RegularPrice[country] == null
                    || !details.RegularPrice[country]!.Equals(rp[country]))
                {
                    return details with {RegularPrice = rp};
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