using System;
using System.Collections.Generic;
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
        public Task<SwitchGamePrice> GetPrice(EshopPriceQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SwitchGamePrice>> GetPrices(EshopMultiPriceQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SwitchGame>> SearchGames(EshopGameQuery query)
        {
            var hch = new HttpClientHandler {Proxy = null, UseProxy = false};
            var http = new HttpClient(hch) {Timeout = TimeSpan.FromSeconds(120)};
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
            var response = await http.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var deserialized = JsonSerializer.Deserialize<LibrarySearchResult>(body);

                if (deserialized != null)
                {
                    return deserialized.Response.Games.Select(ConvertToSwitchGame);
                }
            }

            return new List<SwitchGame>();
        }

        private static SwitchGame ConvertToSwitchGame(LibrarySearchGame game) => new()
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
}