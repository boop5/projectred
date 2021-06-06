using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay("{Amount,nq}", Type = "Regular")]
    internal sealed class PriceSearchRegular
    {
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("raw_value")]
        public string? RawValue { get; set; }
    }
}