using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EzNintendo.Common.Extensions.System.Collections.Generic;
using EzNintendo.Data.Nintendo;
using EzNintendo.Domain.eShop;
using EzNintendo.Domain.Nintendo;
using EzNintendo.Website.Services.Data;
using EzNintendo.Website.Services.Nintendo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Website.Services.Background
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Runtime creates Instance.")]
    public sealed class TrendService
    {
        private readonly ILogger _log;
        private readonly eShopApi _eShop;
        private readonly ApplicationDbContextFactory _ctxFactory;

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter", Justification = "ILogger needs to be generic for DI")]
        public TrendService(ILogger<TrendService> log, eShopApi eShop, ApplicationDbContextFactory ctxFactory)
        {
            _log = log;
            _eShop = eShop;
            _ctxFactory = ctxFactory;
        }

        public async Task UpdateRegion(eShopRegion region, CancellationToken stoppingToken = default)
        {
            foreach (var country in eShopCountryHelper.GetCountriesFromRegion(region))
            {
                _log.LogInformation("Update {country}", country);

                using (_log.BeginScope("[{countryKey}]", eShopCountryHelper.GetKeyFromCountry(country)))
                {
                    await UpdateCountry(country, stoppingToken);
                }

                // todo: log "Finished Updating Country in xxx seconds"
            }
        }

        public async Task UpdateCountry(eShopCountry country, CancellationToken stoppingToken = default)
        {
            static Game getGameFromNsuid(eShopRegion region, IEnumerable<Game> games, NsuId id)
            {
                return games.Single(g => Equals(g.NsuidByRegion(region.ToString()), id));
            }

            static IEnumerable<NsuId> getRegionalIds(eShopRegion region, IEnumerable<Game> games)
            {
                return games.Select(g => g.NsuidByRegion(region.ToString()));
            }

            async Task<IEnumerable<Game>> getRegionalGames(eShopRegion region)
            {
                var library = await _ctxFactory.Get()
                                               .GameQueries
                                               .GetTrendRelevant()
                                               .AsNoTracking()
                                               .ToListAsync(stoppingToken);
                var regionalGames = library.Where(g => g.NsuidByRegion(region.ToString()) != null);

                return regionalGames;
            }

            var tasks = new List<Task>();
            var region = eShopCountryHelper.GetRegionFromCountry(country);
            var games = (await getRegionalGames(region)).ToImmutableList();

            await foreach (var prices in _eShop.GetPrices(country, getRegionalIds(region, games)).WithCancellation(stoppingToken))
            {
                tasks.AddRange(prices.Select(price => UpdateTrend(_ctxFactory.Get(), country, getGameFromNsuid(region, games, price.Nsuid), price, stoppingToken)));
            }

            await Task.WhenAll(tasks);
        }

        private async Task UpdateTrend(DbContext ctx, eShopCountry country, Game game, Price price, CancellationToken stoppingToken = default)
        {
            static Trend getCurrentTrend(Game game, eShopCountry country)
            {
                var countryKey = eShopCountryHelper.GetKeyFromCountry(country);

                return game.Trend
                           .Where(x => x.GameId == game.Id && x.Country == countryKey)
                           .OrderBy(x => x.Created)
                           .LastOrDefault();
                ;
            }

            static Trend buildTrend(Guid gameId, eShopCountry country, Price price)
            {
                var countryKey = eShopCountryHelper.GetKeyFromCountry(country);

                return new Trend { GameId = gameId, Country = countryKey, Price = price.CurrentPrice };
            }

            _log.BeginScope("{game}", game.Title);
            _log.LogDebug("Update Trend");

            try
            {
                var currentTrend = getCurrentTrend(game, country);

                if (currentTrend == null || !Equals(currentTrend.Price, price.CurrentPrice))
                {
                    var currency = eShopCountryHelper.GetCurrencySymbol(eShopCountryHelper.GetCurrencyFromCountry(country));
                    _log.LogInformation("Add new Trend {price}{currency} (last: {previous}{currency})",
                                        price.CurrentPrice,
                                        currency,
                                        currentTrend?.Price.ToString(CultureInfo.InvariantCulture) ?? "-",
                                        currentTrend == null ? string.Empty : currency);

                    await ctx.AddAsync(buildTrend(game.Id, country, price), stoppingToken);
                    await ctx.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception e)
            {
                _log.LogWarning(e, "Failed to Update Trend");
                Debugger.Break();
            }
            finally
            {
                await ctx.DisposeAsync();
            }
        }
    }
}