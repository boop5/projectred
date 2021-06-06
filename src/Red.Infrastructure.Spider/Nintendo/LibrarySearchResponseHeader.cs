using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay("Query: {Parameters.Query,nq} ({QTime,nq}ms)")]
    internal sealed class LibrarySearchResponseHeader
    {
        [JsonPropertyName("params")]
        public LibrarySearchParameters Parameters { get; init; } = new();

        [JsonPropertyName("QTime")]
        public int QTime { get; init; }

        [JsonPropertyName("status")]
        public int Status { get; init; }
    }
}