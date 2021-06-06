using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay("{Amount,nq}", Type = "Discount")]
    internal sealed class PriceSearchDiscount
    {
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("end_datetime")]
        public DateTime? End { get; set; }

        [JsonPropertyName("raw_value")]
        public string? RawValue { get; set; }

        [JsonPropertyName("start_datetime")]
        public DateTime? Start { get; set; }
    }
}