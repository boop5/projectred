using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;
using Red.Infrastructure.NintendoApi.Models;
using Red.Infrastructure.NintendoApi.Util;

namespace Red.Infrastructure.NintendoApi
{
    public class Eshop : IEshop
    {
        public async Task<SwitchGamePrice> GetPrice(EshopPriceQuery query)
        {
            var prices = await GetPrices(new EshopMultiPriceQuery(query.Nsuid));

            return prices.Single();
        }

        public async Task<IReadOnlyCollection<SwitchGamePrice>> GetPrices(EshopMultiPriceQuery query)
        {
            //var url = "https://api.ec.nintendo.com/v1/price?country=DE&lang=en&limit=50&ids=70010000003781&ids=70010000001020&ids=70010000010138&ids=70010000020399
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

        public async Task<IReadOnlyCollection<SwitchGame>> SearchGames(EshopGameQuery query)
        {
            var language = "en";
            var regionUrl = Constants.NintendoEUUrl;

            var baseUrl = $"{regionUrl}/{language}";
            var filter = $"q={query.Term}" +
                         $"&start={query.Index}" +
                         $"&rows={query.Offset}" +
                         "&fq=type:GAME " +
                         "AND ((playable_on_txt: \"HAC\")) " +
                         "AND system_type:nintendoswitch* ";
            // "AND sorting_title:* " +
            // "AND *:*";
            var url = $"{baseUrl}/select?{filter}";
            var response = await GetHttpClient().GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var deserialized = JsonSerializer.Deserialize<LibrarySearchResult>(body);

                if (deserialized != null)
                {
                    return deserialized.Response.Games.Select(ConvertToSwitchGame).ToList();
                }
            }

            return new List<SwitchGame>();
        }

        private static SwitchGame ConvertToSwitchGame(LibrarySearchGame game)
        {
            return new()
            {
                Nsuid = game.Nsuid,
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
                SupportsCloudSave = game.SupportsCloudSave
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

            currentPrice = float.TryParse(price.DiscountPrice?.RawValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var dp) ? dp : regularPrice;

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