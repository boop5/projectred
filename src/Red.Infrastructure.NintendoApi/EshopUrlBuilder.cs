using System.Linq;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;
using Red.Infrastructure.NintendoApi.Util;

namespace Red.Infrastructure.NintendoApi
{
    // todo: games on sale query            https://ec.nintendo.com/api/DE/de/search/sales?count=10&offset=0
    // todo: new games query                https://ec.nintendo.com/api/DE/de/search/new?count=30&offset=0
    // todo: download ranking query         https://ec.nintendo.com/api/DE/de/search/ranking?count=10&offset=0
    // source: https://github.com/cutecore/Nintendo-Switch-eShop-API

    // more:
    // https://searching.nintendo-europe.com/de/select?q=*&start=0&rows=1&fq=*:*&fq=game_series_txt:%22super_smash_bros%22&fq=pg_s:GAME&fq=dates_released_dts:[*%20TO%20NOW]&fq=nsuid_txt:*&sort=score%20desc,%20date_from%20desc&wt=json
    // https://searching.nintendo-europe.com/de/select?q=*&start=0&rows=1&fq=*:*&fq=game_series_txt:%22super_smash_bros%22&fq=pg_s:MERCHANDISE&wt=json

    internal sealed class EshopUrlBuilder
    {
        private IAppLogger<EshopUrlBuilder> Log { get; }

        public EshopUrlBuilder(IAppLogger<EshopUrlBuilder> log)
        {
            Log = log;
        }

        public string BuildGameQueryUrl(EshopGameQuery query)
        {
            // todo: take locale/region from query
            var locale = "en";
            var regionUrl = Constants.NintendoEUUrl;
            var baseUrl = $"{regionUrl}/{locale}/select";
            var filter = $"q={query.Term}" +
                         $"&start={query.Index}" +
                         $"&rows={query.Offset}" +
                         // todo: use sort from query
                         "&sort=dates_released_dts asc" +
                         "&wt=json " +
                         "&bq=!deprioritise_b:true^999 " +
                         "&fq=type:GAME " +
                         "AND ((playable_on_txt: \"HAC\")) " +
                         "AND system_type:nintendoswitch* " +
                         "AND product_code_txt:* " +
                         "AND sorting_title:* " +
                         "AND *:*";
            var url = $"{baseUrl}?{filter}";

            return url;
        }

        public string BuildPriceQueryUrl(EshopMultiPriceQuery query)
        {
            if (query.Nsuids.Count > 50)
            {
                Log.LogWarning("Maximum 50 ids are supported per query {query}", query);
            }

            var q = string.Join(null, query.Nsuids.Take(50).Select(x => $"&ids={x}"));
            // todo: use actual country/language
            var country = "DE";
            var language = "en";
            var url = $"https://api.ec.nintendo.com/v1/price?country={country}&lang={language}&limit=50{q}";

            return url;
        }
    }
}