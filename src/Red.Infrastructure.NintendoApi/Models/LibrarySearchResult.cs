using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.NintendoApi.Models
{
    [DebuggerDisplay(
        "{Response.Games.Count,nq} Games ({ResponseHeader.Parameters.Start,nq}:{ResponseHeader.Parameters.Rows,nq})", 
        Name = "eShop Library",
        Type = "Nintendo eShop Library Search Result")]
    internal sealed class LibrarySearchResult
    {
        [JsonPropertyName("response")]
        public LibrarySearchResponse Response { get; init; } = new();

        [JsonPropertyName("responseHeader")]
        public LibrarySearchResponseHeader ResponseHeader { get; init; } = new();

        [JsonExtensionData]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Dictionary<string, object> _extensionData { get; init; } = new();
    }
}