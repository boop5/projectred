using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay("{Amount,nq}", Type = "Regular")]
    internal sealed class PriceSearchRegular
    {
        [JsonPropertyName("amount")]
        public string? Amount { get; init; }

        [JsonPropertyName("currency")]
        public string? Currency { get; init; }

        [JsonPropertyName("raw_value")]
        public string? RawValue { get; init; }
    }
}