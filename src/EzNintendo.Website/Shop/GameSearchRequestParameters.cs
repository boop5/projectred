using Newtonsoft.Json;

namespace EzNintendo.Website.Shop
{
    public sealed class GameSearchRequestParameters
    {
        [JsonProperty(PropertyName = "q")]
        public string Query { get; set; }

        [JsonProperty(PropertyName = "start")]
        public string Start { get; set; }

        [JsonProperty(PropertyName = "sort")]
        public string Sorting { get; set; }

        [JsonProperty(PropertyName = "fq")]
        public string FullQuery { get; set; }

        [JsonProperty(PropertyName = "rows")]
        public string Rows { get; set; }

        [JsonProperty(PropertyName = "wt")]
        public string Format { get; set; }
    }
}