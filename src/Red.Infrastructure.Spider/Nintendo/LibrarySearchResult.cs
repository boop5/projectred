using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay(
        "{Response.Games.Count,nq} Games ({ResponseHeader.Parameters.Start,nq}:{ResponseHeader.Parameters.Rows,nq})", 
        Name = "eShop Library",
        Type = "Nintendo eShop Library Search Result")]
    internal sealed class LibrarySearchResult
    {
        [JsonPropertyName("response")]
        public LibrarySearchResponse Response { get; set; } = new();

        [JsonPropertyName("responseHeader")]
        public LibrarySearchResponseHeader ResponseHeader { get; set; } = new();
    }
}