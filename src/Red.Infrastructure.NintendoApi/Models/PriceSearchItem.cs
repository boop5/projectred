using System.Diagnostics;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;

namespace Red.Infrastructure.NintendoApi.Models
{
    [DebuggerDisplay(
        "{RegularPrice != null ? \" [REGULAR] \" + RegularPrice.Amount : System.String.Empty,nq}" +
        "{DiscountPrice != null ? \" [DISCOUNT] \" + DiscountPrice.Amount : System.String.Empty,nq}" +
        "{SalesStatus != \"onsale\" ? \" [STATUS] \" + SalesStatus : System.String.Empty,nq}",
        Name = "[{Nsuid,nq}]", Type = "Nintendo eShop Game Price")]
    internal sealed class PriceSearchItem : ExtensionsObject
    {
        [JsonPropertyName("discount_price")]
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public PriceSearchDiscount? DiscountPrice { get; init; }

        [JsonPropertyName("title_id")]
        [JsonConverter(typeof(LongJsonConverter))] // todo: convert to string
        public long Nsuid { get; init; }

        [JsonPropertyName("regular_price")]
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public PriceSearchRegular RegularPrice { get; init; } = new();

        [JsonPropertyName("sales_status")]
        public string? SalesStatus { get; init; }

        [JsonPropertyName("is_download_ready")]
        public bool? IsDownloadReady { get; init; }
    }
}