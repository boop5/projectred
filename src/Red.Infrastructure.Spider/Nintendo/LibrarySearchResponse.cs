using System.Collections.Generic;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;

namespace Red.Infrastructure.Spider.Nintendo
{
    internal sealed class LibrarySearchResponse
    {
        [JsonPropertyName("numFound")]
        public int FoundGames { get; set; }

        [JsonPropertyName("docs")]
        public IEnumerable<LibrarySearchGame> Games { get; set; } = new List<LibrarySearchGame>();

        [JsonPropertyName("start")]
        [JsonConverter(typeof(IntJsonConverter))]
        public int Start { get; set; }
    }
}