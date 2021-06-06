using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay("[{Country,nq}] {Prices.Count,nq} Prices")]
    internal sealed class PriceSearchResult
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = "";

        [JsonPropertyName("personalized")]
        public bool Personalized { get; set; }

        [JsonPropertyName("prices")]
        public List<PriceSearchItem> Prices { get; init; } = new();
    }

    // [DebuggerDisplay("{RegularPrice.Amount,nq} {SalesStatus,nq} {DiscountPrice != null ? DiscountPrice.Amount : System.String.Empty,nq}", Name = "[{Nsuid,nq}]")]
}