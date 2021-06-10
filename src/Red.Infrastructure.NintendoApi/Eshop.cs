using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Red.Core.Application.Extensions;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;
using Red.Infrastructure.NintendoApi.Models;

namespace Red.Infrastructure.NintendoApi
{
    internal sealed class Eshop : IEshop
    {
        private readonly EshopConverter _converter;
        private readonly EshopHttpClient _http;
        private readonly EshopUrlBuilder _urlBuilder;
        private ILogger<Eshop> Log { get; }

        public Eshop(ILogger<Eshop> log,
                     EshopHttpClient http,
                     EshopUrlBuilder urlBuilder,
                     EshopConverter converter)
        {
            Log = log;
            _http = http;
            _urlBuilder = urlBuilder;
            _converter = converter;
        }

        public async Task<IReadOnlyCollection<SwitchGamePrice>> GetPrices(EshopMultiPriceQuery query)
        {
            try
            {
                var url = _urlBuilder.BuildPriceQueryUrl(query);
                var searchResult = await _http.GetAs<PriceSearchResult>(url);

                if (searchResult != null)
                {
                    return searchResult.Prices.Select(_converter.ConvertToSwitchGamePrice).ToList();
                }
            }
            catch (Exception e)
            {
                Log.LogWarning(e, "Failed to get prices");
            }

            return new List<SwitchGamePrice>();
        }

        public async Task<int> GetTotalGames()
        {
            var searchResult = await GetLibrary(new EshopGameQuery {Index = 0, Offset = 1});

            if (searchResult != null)
            {
                return searchResult.Response.FoundGames;
            }

            return 0;
        }

        public async Task<IReadOnlyCollection<SwitchGame>> SearchGames(EshopGameQuery query)
        {
            var searchResult = await GetLibrary(query);

            if (searchResult != null)
            {
                return searchResult.Response.Games
                                   .Where(x => x.ProductCodeSS?.Count == 1)
                                   // some games have duplicate entries in the result, so lets remove them
                                   .DistinctBy(x => x.ProductCodeSS![0])
                                   .Select(_converter.ConvertToSwitchGame)
                                   .ToList();
            }

            return new List<SwitchGame>();
        }

        private async Task<LibrarySearchResult?> GetLibrary(EshopGameQuery query)
        {
            try
            {
                var url = _urlBuilder.BuildGameQueryUrl(query);
                return await _http.GetAs<LibrarySearchResult>(url);
            }
            catch (Exception e)
            {
                Log.LogWarning(e, "Failed to get library");
            }

            return null;
        }
    }
}