using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay("{Games.Count,nq} Games")]
    internal sealed class LibrarySearchResponse
    {
        [JsonPropertyName("numFound")]
        [JsonConverter(typeof(IntJsonConverter))]
        public int FoundGames { get; set; }

        [JsonPropertyName("docs")]
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public List<LibrarySearchGame> Games { get; init; } = new();

        [JsonPropertyName("start")]
        [JsonConverter(typeof(IntJsonConverter))]
        public int Start { get; set; }
    }
}