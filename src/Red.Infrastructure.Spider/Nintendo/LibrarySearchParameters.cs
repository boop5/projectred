using System.Diagnostics;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay("{FullQuery,nq}")]
    internal sealed class LibrarySearchParameters
    {
        [JsonPropertyName("wt")]
        public string Format { get; init; } = "";

        [JsonPropertyName("fq")]
        public string FullQuery { get; init; } = "";

        [JsonPropertyName("q")]
        public string Query { get; init; } = "";

        [JsonPropertyName("rows")]
        [JsonConverter(typeof(IntJsonConverter))]
        public int Rows { get; init; }

        [JsonPropertyName("sort")]
        public string Sorting { get; init; } = "";

        [JsonPropertyName("start")]
        [JsonConverter(typeof(IntJsonConverter))]
        public int Start { get; init; }
    }
}