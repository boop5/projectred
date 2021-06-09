using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;

namespace Red.Infrastructure.NintendoApi.Models
{
    [DebuggerDisplay("{Games.Count,nq} Games")]
    internal sealed class LibrarySearchResponse
    {
        [JsonExtensionData]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public ReadOnlyDictionary<string, object> _extensionData { get; init; } 
            = new(new Dictionary<string, object>());

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