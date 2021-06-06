﻿using System.Diagnostics;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay(
        "[REGULAR] {RegularPrice.Amount, nq}" +
        "{DiscountPrice != null ? \" [DISCOUNT] \" + DiscountPrice.Amount : System.String.Empty,nq}" +
        "{SalesStatus != \"onsale\" ? \" [STATUS] \" + SalesStatus : System.String.Empty,nq}",
        Name = "[{Nsuid,nq}]", Type = "Nintendo eShop Game Price")]
    internal sealed class PriceSearchItem
    {
        [JsonPropertyName("discount_price")]
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public PriceSearchDiscount? DiscountPrice { get; set; }

        [JsonPropertyName("title_id")]
        [JsonConverter(typeof(LongJsonConverter))]
        public long Nsuid { get; set; }

        [JsonPropertyName("regular_price")]
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public PriceSearchRegular RegularPrice { get; set; } = new();

        [JsonPropertyName("sales_status")]
        public string? SalesStatus { get; set; }
    }
}