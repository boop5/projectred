using System.Linq;
using Red.Core.Application.Extensions;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.NintendoApi
{
    // todo: new games query                https://ec.nintendo.com/api/DE/de/search/new?count=30&offset=0
    // todo: download ranking query         https://ec.nintendo.com/api/DE/de/search/ranking?count=10&offset=0
    // source: https://github.com/cutecore/Nintendo-Switch-eShop-API

    // more:
    // fq types: JOBOFFER, STANDARD, NEWS, DLC, MERCHANDISE, GAME, SUPPORT, EVENT, FIGURE, INTERVIEW
    // https://searching.nintendo-europe.com/de/select?q=*&start=0&rows=1&fq=*:*&fq=game_series_txt:%22super_smash_bros%22&fq=pg_s:GAME&fq=dates_released_dts:[*%20TO%20NOW]&fq=nsuid_txt:*&sort=score%20desc,%20date_from%20desc&wt=json
    // https://searching.nintendo-europe.com/de/select?q=*&start=0&rows=1&fq=*:*&fq=game_series_txt:%22super_smash_bros%22&fq=pg_s:MERCHANDISE&wt=json

    internal sealed class EshopUrlBuilder
    {
        private IAppLogger<EshopUrlBuilder> Log { get; }

        public EshopUrlBuilder(IAppLogger<EshopUrlBuilder> log)
        {
            Log = log;
        }

        public string BuildSalesQueryUrl(EshopSalesQuery query)
        {
            var region = query.Culture.GetTwoLetterISORegionName();
            var locale = query.Culture.TwoLetterISOLanguageName;
            var url = $"https://ec.nintendo.com/api/{region}/{locale}/search/sales"
                      + $"?count={query.Count}&offset={query.Offset}";

            return url;
        }

        public string BuildGameQueryUrl(EshopGameQuery query)
        {
            var locale = query.Culture.TwoLetterISOLanguageName;
            var baseUrl = $"https://searching.nintendo-europe.com/{locale}/select";
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
            var country = query.Culture.GetTwoLetterISORegionName();
            var language = query.Culture.TwoLetterISOLanguageName;
            var url = $"https://api.ec.nintendo.com/v1/price?country={country}&lang={language}&limit=50{q}";

            return url;
        }
    }
}