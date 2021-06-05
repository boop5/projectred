using Newtonsoft.Json;

namespace EzNintendo.Website.Shop
{
    public sealed class SearchResponseHeader
    {
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "QTime")]
        public int QTime { get; set; }

        [JsonProperty(PropertyName = "params")]
        public GameSearchRequestParameters Parameters { get; set; }
    }
}