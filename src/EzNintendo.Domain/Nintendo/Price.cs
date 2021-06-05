using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using EzNintendo.Domain.Converter;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Nintendo
{
    [DebuggerDisplay("{Nsuid.Id,nq}", Name = "{CurrentPrice,nq}")]
    public sealed class Price
    {
        public float CurrentPrice => DiscountPrice.HasValue ? MathF.Min(DiscountPrice.Value, RegularPrice) : RegularPrice;

        [JsonProperty("title_id")]
        [JsonConverter(typeof(NsuidConverter))]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public NsuId Nsuid { get; set; }

        [JsonProperty("regular_price")]
        [JsonConverter(typeof(RegularPriceConverter))]
        public float RegularPrice { get; set; }

        [JsonProperty("discount_price")]
        [JsonConverter(typeof(DiscountPriceConverter))]
        public float? DiscountPrice { get; set; }
    }

    internal class RegularPrice
    {
        [JsonProperty("raw_value")]
        public string? RawValue { get; set; }

        [JsonProperty("amount")]
        public string? Amount { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }
    }

    internal class DiscountPrice : RegularPrice
    {
        [JsonProperty("start_datetime")]
        public DateTime Start { get; set; }

        [JsonProperty("end_datetime")]
        public DateTime End { get; set; }
    }
}