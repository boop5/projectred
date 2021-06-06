using System.Text.Json.Serialization;
using Red.Core.Application.Json;

namespace Red.Infrastructure.Spider.Nintendo
{
    internal sealed class LibrarySearchParameters
    {
        [JsonPropertyName("wt")]
        public string Format { get; set; }  = "";

        [JsonPropertyName("fq")]
        public string FullQuery { get; set; }  = "";

        [JsonPropertyName("q")]
        public string Query { get; set; } = "";

        [JsonPropertyName("rows")]
        [JsonConverter(typeof(IntJsonConverter))]
        public int Rows { get; set; } 

        [JsonPropertyName("sort")]
        public string Sorting { get; set; }  = "";

        [JsonPropertyName("start")]
        [JsonConverter(typeof(IntJsonConverter))]
        public int Start { get; set; }
    }
}