using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay("[{Country,nq}] {Prices.Count,nq} Prices", Type = "Nintendo eShop Price Search Result")]
    internal sealed class PriceSearchResult
    {
        [JsonPropertyName("country")]
        [DebuggerDisplay("{Country,nq}", Type = "ISO 3166-1 alpha-2")]
        public string Country { get; set; } = "";

        [JsonPropertyName("personalized")]
        public bool Personalized { get; set; }

        [JsonPropertyName("prices")]
        public List<PriceSearchItem> Prices { get; init; } = new();
    }
}