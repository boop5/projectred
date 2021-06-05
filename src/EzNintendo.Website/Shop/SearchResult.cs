using Newtonsoft.Json;

namespace EzNintendo.Website.Shop
{
    internal class SearchResult
    {
        [JsonProperty(PropertyName = "responseHeader")]
        public SearchResponseHeader ResponseHeader { get; set; }

        [JsonProperty(PropertyName = "response")]
        public GameSearchResponse Response { get; set; }
    }
}