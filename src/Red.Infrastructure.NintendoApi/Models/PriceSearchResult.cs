using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.NintendoApi.Models
{
    [DebuggerDisplay("[{Country,nq}] {Prices.Count,nq} Prices", Type = "Nintendo eShop Price Search Result")]
    internal sealed class PriceSearchResult : ExtensionsObject
    {
        [JsonPropertyName("country")]
        [DebuggerDisplay("{Country,nq}", Type = "ISO 3166-1 alpha-2")]
        public string Country { get; init; } = "";

        [JsonPropertyName("personalized")]
        public bool Personalized { get; init; }

        [JsonPropertyName("prices")]
        public List<PriceSearchItem> Prices { get; init; } = new();
    }
}