using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Red.Infrastructure.Spider.Json;

namespace Red.Infrastructure.Spider.Nintendo
{
    public sealed class LibrarySearchGame
    {
        [JsonPropertyName("excerpt")]
        public string? Excerpt { get; set; }

        [JsonPropertyName("fs_id")]
        public long? FsId { get; set; }

        [JsonPropertyName("nsuid_txt")]
        [JsonConverter(typeof(NsuidListJsonConverter))]
        public string? NsUid { get; set; }

        [JsonPropertyName("publisher")]
        public string? Publisher { get; set; }

        [JsonPropertyName("price_regular_f")]
        public float? RegularPrice { get; set; }

        [JsonPropertyName("dates_released_dts")]
        [JsonConverter(typeof(ReleaseDateJsonConverter))]
        public DateTime? ReleaseDate { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }


        #region other

        [JsonPropertyName("add_on_content_b ")]
        public bool? AddOnContentAvailable { get; set; }

        [JsonPropertyName("change_date")]
        public DateTime? Changed { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("club_nintendo")]
        public bool? ClubNintendo { get; set; }

        [JsonPropertyName("hd_rumble_b")]
        public bool? HdRumble { get; set; }

        [JsonPropertyName("multiplayer_mode")]
        public string? MultiplayerMode { get; set; }

        [JsonPropertyName("play_mode_tv_mode_b")]
        public bool? TvMode { get; set; }

        [JsonPropertyName("play_mode_handheld_mode_b")]
        public bool? HandheldMode { get; set; }

        [JsonPropertyName("product_code_txt")]
        [JsonConverter(typeof(FirstItemJsonConverter))]
        public string? ProductCode { get; set; }

        // [JsonPropertyName("compatible_controller")]
        // [JsonConverter(typeof(CompatibleControllerConverter))]
        // public List<string>? CompatibleController { get; set; }

        [JsonPropertyName("paid_subscription_required_b")]
        public bool? SubscriptionRequired { get; set; }

        [JsonPropertyName("cloud_saves_b")]
        public bool? SupportsCloudSave { get; set; }

        [JsonPropertyName("age_rating_sorting_i")]
        public byte? AgeRating { get; set; }

        [JsonPropertyName("age_rating_type")]
        public string? AgeRatingType { get; set; }

        [JsonPropertyName("age_rating_value")]
        public string? AgeRatingValue { get; set; }

        [JsonPropertyName("play_mode_tabletop_mode_b")]
        public bool? TabletopMode { get; set; }

        [JsonPropertyName("language_availability")]
        [JsonConverter(typeof(LanguagesJsonConverter))]
        public List<string>? Languages { get; set; }

        [JsonPropertyName("price_has_discount_b")]
        public bool? Discount { get; set; }

        [JsonPropertyName("price_discount_percentage_f")]
        public float? DiscountPercentage { get; set; }

        [JsonPropertyName("sorting_title")]
        public string? SortingTitle { get; set; }

        [JsonPropertyName("voice_chat_b")]
        public bool? VoiceChat { get; set; }

        [JsonPropertyName("developer")]
        public string? Developer { get; set; }

        [JsonPropertyName("physical_version_b")]
        public bool? PhysicalVersion { get; set; }

        [JsonPropertyName("digital_version_b")]
        public bool? DigitalVersion { get; set; }

        [JsonPropertyName("price_lowest_f")]
        // [JsonConverter(typeof(StringToDecimalConverter))]
        public decimal? LowestPrice { get; set; }

        [JsonPropertyName("players_from")]
        public byte? MinPlayers { get; set; }

        [JsonPropertyName("players_to")]
        // [JsonConverter(typeof(StringToByteConverter))]
        public byte? MaxPlayers { get; set; }

        [JsonPropertyName("pretty_game_categories_txt")]
        public List<string>? Categories { get; set; }

        // ReSharper disable InconsistentNaming
        public string? image_url { get; set; } // ImageSquare
        public string? image_url_sq_s { get; set; }
        public string? image_url_h2x1_s { get; set; }
        public string? wishlist_email_square_image_url_s { get; set; }
        public string? wishlist_email_banner640w_image_url_s { get; set; } // ImageWide
        public string? wishlist_email_banner460w_image_url_s { get; set; }
        public string? gift_finder_wishlist_image_url_s { get; set; }
        public string? gift_finder_detail_page_image_url_s { get; set; }
        // ReSharper restore InconsistentNaming

        #endregion
    }
}