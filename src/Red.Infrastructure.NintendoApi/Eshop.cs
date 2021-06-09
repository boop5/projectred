using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Red.Core.Application.Extensions;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;
using Red.Infrastructure.NintendoApi.Models;
using Red.Infrastructure.NintendoApi.Util;

namespace Red.Infrastructure.NintendoApi
{
    // https://github.com/cutecore/Nintendo-Switch-eShop-API
    // todo: sales query            https://ec.nintendo.com/api/DE/de/search/sales?count=10&offset=0
    // todo: new query              https://ec.nintendo.com/api/DE/de/search/new?count=30&offset=0
    // todo: download ranking query https://ec.nintendo.com/api/DE/de/search/ranking?count=10&offset=0

    public class Eshop : IEshop
    {
        private readonly ISlugBuilder _slugBuilder;

        public Eshop(IEshopSlugBuilder slugBuilder)
        {
            _slugBuilder = slugBuilder;
        }

        public async Task<SwitchGamePrice> GetPrice(EshopPriceQuery query)
        {
            var prices = await GetPrices(new EshopMultiPriceQuery(query.Nsuid));

            return prices.Single();
        }

        public async Task<IReadOnlyCollection<SwitchGamePrice>> GetPrices(EshopMultiPriceQuery query)
        {
            var q = string.Join(null, query.Nsuids.Take(50).Select(x => $"&ids={x}"));
            var country = "DE";
            var language = "en";
            var url = $"https://api.ec.nintendo.com/v1/price?country={country}&lang={language}&limit=50{q}";
            var response = await GetHttpClient().GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var deserialized = JsonSerializer.Deserialize<PriceSearchResult>(body);

                if (deserialized != null)
                {
                    return deserialized.Prices.Select(ConvertToSwitchGamePrice).ToList();
                }
            }

            return new List<SwitchGamePrice>();
        }

        public async Task<int> GetTotalGames()
        {
            var search = await GetLibrary(new EshopGameQuery {Index = 0, Offset = 1});

            if (search != null)
            {
                return search.Response.FoundGames;
            }

            return 0;
        }

        public async Task<IReadOnlyCollection<SwitchGame>> SearchGames(EshopGameQuery query)
        {
            var library = await GetLibrary(query);

            if (library != null)
            {
                return library.Response.Games
                              .Where(x => x.ProductCodeSS?.Count == 1)
                              // some games have duplicate entries in the result, so lets remove them
                              .DistinctBy(x => x.ProductCodeSS![0])
                              .Select(ConvertToSwitchGame)
                              .ToList();
            }

            return new List<SwitchGame>();
        }

        private string? BuildSlug(LibrarySearchGame game)
        {
            if (string.IsNullOrWhiteSpace(game.Title))
            {
                return null;
            }

            return _slugBuilder.Build(game.Title);
        }

        private SwitchGame ConvertToSwitchGame(LibrarySearchGame game)
        {
            return new()
            {
                Nsuids = game.Nsuids,
                AgeRating = game.AgeRating,
                Categories = game.GameCategories,
                Coop = game.CoopPlay,
                DemoAvailable = game.DemoAvailable,
                Developer = game.Developer,
                Publisher = game.Publisher,
                VoucherPossible = game.SwitchGameVoucher,
                Description = game.Excerpt,
                Languages = game.Languages,
                MinPlayers = game.MinPlayers,
                MaxPlayers = game.MaxPlayers,
                Popularity = game.Popularity ?? 0,
                ReleaseDate = game.ReleaseDate,
                RemovedFromEshop = game.RemovedFromEshop,
                Title = game.Title,
                SupportsCloudSave = game.SupportsCloudSave,
                Slug = BuildSlug(game),
                // todo: Insert actual region
                Region = "EU",
                ProductCode = game.ProductCodeSS![0]
                // todo: add missing fields
            };
        }

        private static SwitchGamePrice ConvertToSwitchGamePrice(PriceSearchItem price)
        {
            float? regularPrice = null;
            float? currentPrice;

            if (float.TryParse(price.RegularPrice.RawValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var rp))
            {
                regularPrice = rp;
            }

            currentPrice = float.TryParse(
                price.DiscountPrice?.RawValue,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out var dp)
                ? dp
                : regularPrice;

            // todo: remove .ToString()
            return new SwitchGamePrice(price.Nsuid!.ToString())
            {
                SalesStatus = StringToSalesStatus(price.SalesStatus),
                RegularPrice = regularPrice,
                CurrentPrice = currentPrice,
                Currency = price.RegularPrice.Currency,
                Discounted = currentPrice < regularPrice
            };
        }

        private static HttpClient GetHttpClient()
        {
            var hch = new HttpClientHandler {Proxy = null, UseProxy = false};
            var http = new HttpClient(hch) {Timeout = TimeSpan.FromSeconds(120)};
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return http;
        }

        private async Task<LibrarySearchResult?> GetLibrary(EshopGameQuery query)
        {
            var locale = "en";
            var regionUrl = Constants.NintendoEUUrl;

            var baseUrl = $"{regionUrl}/{locale}/select";
            var filter = $"q={query.Term}" +
                         $"&start={query.Index}" +
                         $"&rows={query.Offset}" +
                         "&sort=dates_released_dts asc" +
                         // "&sort=sorting_title asc" +
                         "&wt=json " +
                         "&bq=!deprioritise_b:true^999 " +
                         "&fq=type:GAME " +
                         "AND ((playable_on_txt: \"HAC\")) " +
                         "AND system_type:nintendoswitch* " +
                         "AND product_code_txt:* " +
                         "AND sorting_title:* " +
                         "AND *:*";
            var url = $"{baseUrl}?{filter}";
            var response = await GetHttpClient().GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var deserialized = JsonSerializer.Deserialize<LibrarySearchResult>(body);

                return deserialized;
            }

            return null;
        }

        private static EshopSalesStatus StringToSalesStatus(string? text)
        {
            if (string.Equals(text, "onsale", StringComparison.InvariantCultureIgnoreCase))
            {
                return EshopSalesStatus.OnSale;
            }

            if (string.Equals(text, "sales_termination", StringComparison.InvariantCultureIgnoreCase))
            {
                return EshopSalesStatus.SalesTermination;
            }

            if (string.Equals(text, "not_found", StringComparison.InvariantCultureIgnoreCase))
            {
                return EshopSalesStatus.NotFound;
            }

            if (string.Equals(text, "unreleased", StringComparison.InvariantCultureIgnoreCase))
            {
                return EshopSalesStatus.Unreleased;
            }

            Debugger.Break();
            return EshopSalesStatus.Unknown;
        }
    }
}