using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;

namespace Red.Infrastructure.NintendoApi.Models
{
    [DebuggerDisplay("{Games.Count,nq} Games")]
    internal sealed class LibrarySearchResponse : ExtensionsObject
    {
        [JsonPropertyName("numFound")]
        [JsonConverter(typeof(IntJsonConverter))]
        public int FoundGames { get; init; }

        [JsonPropertyName("docs")]
        public List<LibrarySearchGame> Games { get; init; } = new();

        [JsonPropertyName("start")]
        [JsonConverter(typeof(IntJsonConverter))]
        public int Start { get; init; }
    }
}