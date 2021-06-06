using System.Text.Json.Serialization;

namespace Red.Infrastructure.Spider.Nintendo
{
    internal sealed class LibrarySearchResult
    {
        [JsonPropertyName("response")]
        public LibrarySearchResponse Response { get; set; } = new();

        [JsonPropertyName("responseHeader")]
        public LibrarySearchResponseHeader ResponseHeader { get; set; } = new();
    }
}