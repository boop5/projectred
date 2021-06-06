using System.Text.Json.Serialization;

namespace Red.Infrastructure.Spider.Nintendo
{
    internal sealed class LibrarySearchResponseHeader
    {
        [JsonPropertyName("params")]
        public LibrarySearchParameters Parameters { get; set; } = new();

        [JsonPropertyName("QTime")]
        public int QTime { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }
    }
}