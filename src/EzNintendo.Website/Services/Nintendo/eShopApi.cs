using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using EzNintendo.Common.Extensions.System.Collections.Generic;
using EzNintendo.Common.Utilities;
using EzNintendo.Domain.eShop;
using EzNintendo.Domain.Nintendo;
using EzNintendo.Website.Services.Web;
using EzNintendo.Website.Shop;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EzNintendo.Website.Services.Nintendo
{
    /// <summary>
    ///     API to access Information from the official Nintendo eShop.
    /// </summary>
    /// <remarks>
    ///     https://github.com/lmmfranco/nintendo-switch-eshop
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed class eShopApi
    {
        private readonly ILogger<eShopApi> _log;
        private readonly HttpService _httpService;
        private readonly GameSearchQueryBuilder _gameQueryBuilder;

        public eShopApi(ILogger<eShopApi> log, HttpService httpService, GameSearchQueryBuilder gameQueryBuilder)
        {
            _log = log;
            _httpService = httpService;
            _gameQueryBuilder = gameQueryBuilder;

            _log.LogTrace("Instance created.");
        }

        public async IAsyncEnumerable<IList<Price>> GetPrices(eShopCountry country, IEnumerable<NsuId> nsuids)
        {
            foreach (var chunk in nsuids.ChunkBy(50))
            {
                var queryBuilder = new PriceSearchQueryBuilder(eShopCountryHelper.GetKeyFromCountry(country));
                var url = queryBuilder.Build(chunk);
                var response = await _httpService.GetAsync(url);
                var deserialized = JsonConvert.DeserializeObject<PriceSearchResponse>(response);

                yield return deserialized.Prices;
            }
        }

        public async Task<IEnumerable<Price>> GetPricesAsync(eShopCountry country, IEnumerable<NsuId> nsuids)
        {
            var result = new List<Price>();

            foreach (var chunk in nsuids.ChunkBy(50))
            {
                var queryBuilder = new PriceSearchQueryBuilder(eShopCountryHelper.GetKeyFromCountry(country));
                var url = queryBuilder.Build(chunk);
                var response = await _httpService.GetAsync(url);
                var deserialized = JsonConvert.DeserializeObject<PriceSearchResponse>(response);

                result.AddRange(deserialized.Prices);
            }

            return result;
        }

        public async Task<List<GameDTO>> GetAllGames(eShopRegion region)
        {
            _log.LogTrace("Get All Games from NintendoAPI.");

            var url = _gameQueryBuilder.QueryAll(region);
            var response = await _httpService.GetAsync(url);
            var deserialized = JsonConvert.DeserializeObject<SearchResult>(response);

            return deserialized.Response.Games.Where(g => g != null).ToList();
        }
    }
}