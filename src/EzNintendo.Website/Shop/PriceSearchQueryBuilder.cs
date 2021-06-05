using System.Collections.Generic;
using System.Linq;
using EzNintendo.Domain;
using EzNintendo.Domain.Nintendo;

namespace EzNintendo.Website.Shop
{
    internal class PriceSearchQueryBuilder
    {
        private readonly string _country;
        private readonly string _language;

        public PriceSearchQueryBuilder(string country, string language = "en")
        {
            _country = country;
            _language = language;
        }

        public string Build(IEnumerable<NsuId> games)
        {
            var baseUrl = "https://api.ec.nintendo.com/v1/price";

            var ids = string.Join(string.Empty, games.ToList().Select(id => $"&ids={id.Id}"));
            var url = $"{baseUrl}?country={_country}&lang={_language}&limit=50{ids}";
            //var url = $"{baseUrl}?country={Country}&lang={Locale}&limit=50{ids}";
            //https://api.ec.nintendo.com/v1/price?country=DE&ids=70010000003781&ids=70010000001020&ids=70010000010138&lang=en&limit=50

            return url;
        }
    }
}