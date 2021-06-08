using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.NintendoApi.Models
{
    [DebuggerDisplay("{Amount,nq}", Type = "Discount")]
    internal sealed class PriceSearchDiscount
    {
        [JsonPropertyName("amount")]
        public string? Amount { get; init; }

        [JsonPropertyName("currency")]
        public string? Currency { get; init; }

        [JsonPropertyName("end_datetime")]
        public DateTime? End { get; init; }

        [JsonPropertyName("raw_value")]
        public string? RawValue { get; init; }

        [JsonPropertyName("start_datetime")]
        public DateTime? Start { get; init; }
    }
}