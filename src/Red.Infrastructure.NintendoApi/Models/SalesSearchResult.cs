using System.Collections.Generic;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;
using Red.Infrastructure.NintendoApi.Json;

namespace Red.Infrastructure.NintendoApi.Models
{
    internal sealed class SalesSearchResult : ExtensionsObject
    {
        [JsonPropertyName("length")]
        public int? Length { get; init; }

        [JsonPropertyName("offset")]
        public int? Offset { get; init; }

        [JsonPropertyName("total")]
        public int? Total { get; init; }

        [JsonPropertyName("contents")]
        public List<SalesSearchItem> Contents { get; init; } = new();
    }

    internal sealed class SalesSearchContentDescriptor : ExtensionsObject
    {
        [JsonPropertyName("id")]
        public int? Id { get; init; }

        [JsonPropertyName("name")]
        public string? Name{ get; init; }

        [JsonPropertyName("type")]
        public string? Type { get; init; }
    }

    internal sealed class SalesSearchRating : ExtensionsObject
    {
        [JsonPropertyName("age")]
        public int? Age { get; init; }

        [JsonPropertyName("id")]
        public int? Id { get; init; }

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; init; }

        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("provisional")]
        public bool? Provisional { get; init; }

        [JsonPropertyName("svg_image_url")]
        public string? SvgImageUrl { get; init; }
    }

    internal sealed class SalesSearchRatingSystem : ExtensionsObject
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("id")]
        public int? Id { get; init; }
    }

    internal sealed class SalesSearchRatingInfo : ExtensionsObject
    {
        [JsonPropertyName("content_descriptors")]
        public List<SalesSearchContentDescriptor> ContentDescriptors { get; init; } = new();

        [JsonPropertyName("rating")]
        public SalesSearchRating? Rating { get; init; }

        [JsonPropertyName("rating_system")]
        public SalesSearchRatingSystem? RatingSystem { get; init; }
    }

    internal sealed class SalesSearchItem : ExtensionsObject
    {
        // todo: disclaimer
        // todo: strong_disclaimer

        [JsonPropertyName("content_type")]
        public string? ContentType { get; init; }

        [JsonPropertyName("dominant_colors")]
        // todo: ggf converter?
        public List<string> DominantColors { get; init; } = new();

        [JsonPropertyName("formal_name")]
        public string? FormalName { get; init; }

        [JsonPropertyName("hero_banner_url")]
        public string? HeroBannerUrl { get; init; }

        [JsonPropertyName("id")]
        [JsonConverter(typeof(NumberToStringJsonConverter))]
        public string? Nsuid { get; init; }

        [JsonPropertyName("is_new")]
        public bool? IsNew { get; init; }

        [JsonPropertyName("membership_required")]
        public bool? MembershipRequired { get; init; }

        [JsonPropertyName("public_status")]
        public string? PublicStatus { get; init; }

        [JsonPropertyName("rating_info")]
        public SalesSearchRatingInfo? RatingInfo { get; init; }

        [JsonPropertyName("release_date_on_eshop")]
        public string? ReleaseDate { get; init; }

        [JsonPropertyName("screenshots")]
        [JsonConverter(typeof(SalesSearchScreenshotsJsonConverter))]
        public List<string> Screenshots { get; init; } = new();

        // [JsonPropertyName("tags")]
        // public List<???> Tags { get; init; } = new();
        
        [JsonPropertyName("target_titles")]
        public List<string> TargetTitles { get; init; } = new();
    }
}