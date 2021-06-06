using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;
using Red.Infrastructure.Spider.Json;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay("{Title,nq}", Name = "[{Nsuid,nq}]", Type = "Nintendo eShop Game")]
    public sealed class LibrarySearchGame
    {
        #region new

        /// <summary>
        ///     The same as <see cref="XXX_GameSeries_T"/> - just as array. No idea why 🤷‍
        /// </summary>
        [JsonPropertyName("game_series_txt")]
        [JsonConverter(typeof(ListJsonConverter<string>))]
        public List<string> XXX_GameSeries { get; set; }

        [JsonPropertyName("game_series_t")]
        public string XXX_GameSeries_T { get; set; }

        [JsonPropertyName("pretty_date_s")]
        public string XXX_PrettyDate { get; set; }

        [JsonPropertyName("deprioritise_b")]
        public bool XXX_Deprioritise { get; set; }

        [JsonPropertyName("pg_s")]
        public string XXX_PG { get; set; }

        [JsonPropertyName("add_on_content_b")]
        public bool XXX_AddOnContent { get; set; }

        [JsonPropertyName("near_field_comm_b")]
        public bool XXX_NFC { get; set; }

        [JsonPropertyName("originally_for_t")]
        public string XXX_OriginallyFor { get; set; }

        [JsonPropertyName("priority")]
        public string XXX_Priority { get; set; }

        [JsonPropertyName("title_extras_txt")]
        [JsonConverter(typeof(ListJsonConverter<string>))]
        public List<string> XXX_TitleExtras { get; set; }

        [JsonPropertyName("labo_b")]
        public bool XXX_Labo { get; set; }

        [JsonPropertyName("system_type")]
        [JsonConverter(typeof(FirstItemJsonConverter))]
        public string XXX_SystemType { get; set; }

        [JsonPropertyName("game_categories_txt")]
        [JsonConverter(typeof(ListJsonConverter<string>))]
        public List<string> XXX_GameCategories { get; set; }

        [JsonPropertyName("product_code_ss")]
        [JsonConverter(typeof(FirstItemJsonConverter))]
        public string XXX_ProductCodeSS { get; set; }

        [JsonPropertyName("date_from")]
        public DateTime XXX_DateFrom { get; set; }

        [JsonPropertyName("dlc_shown_b")]
        public bool XXX_DlcShown { get; set; }

        [JsonPropertyName("image_url_tm_s")]
        public string XXX_ImageTrademark { get; set; }

        [JsonPropertyName("gift_finder_description_s")]
        public string XXX_GiftFinderDescription { get; set; }

        [JsonPropertyName("copyright_s")]
        public string XXX_copyright_s { get; set; }
        [JsonPropertyName("gift_finder_carousel_image_url_s")]
        public string XXX_gift_finder_carousel_image_url_s { get; set; }
        [JsonPropertyName("playable_on_txt")]
        [JsonConverter(typeof(ListJsonConverter<string>))]
        public List<string> XXX_playable_on_txt { get; set; }
        [JsonPropertyName("hits_i")]
        public int XXX_hits_i { get; set; }
        
        [JsonPropertyName("switch_game_voucher_b")]
        public bool XXX_switch_game_voucher_b { get; set; }

        [JsonPropertyName("game_category")]
        [JsonConverter(typeof(ListJsonConverter<string>))]
        public List<string> XXX_game_category { get; set; }
        [JsonPropertyName("system_names_txt")]
        [JsonConverter(typeof(ListJsonConverter<string>))]
        public List<string> XXX_system_names_txt { get; set; }
        [JsonPropertyName("pretty_agerating_s")]
        public string XXX_pretty_agerating_s { get; set; }

        [JsonPropertyName("eshop_removed_b")]
        public bool XXX_eshop_removed_b { get; set; }
        
        [JsonPropertyName("gift_finder_detail_page_store_link_s")]
        public string XXX_gift_finder_detail_page_store_link_s { get; set; }
        [JsonPropertyName("price_sorting_f")]
        public float XXX_price_sorting_f { get; set; }
        [JsonPropertyName("_version_")]
        public long XXX__version_ { get; set; }
        [JsonPropertyName("popularity")]
        public int XXX_popularity { get; set; }
        [JsonPropertyName("demo_availability")]
        public bool XXX_demo_availability { get; set; }
        [JsonPropertyName("internet")]
        public bool XXX_internet { get; set; }
        [JsonPropertyName("mii_support")]
        public bool XXX_mii_support { get; set; }
        [JsonPropertyName("ir_motion_camera_b")]
        public bool XXX_ir_motion_camera_b { get; set; }
        [JsonPropertyName("local_play")]
        public bool XXX_local_play { get; set; }
        [JsonPropertyName("coop_play_b")]
        public bool XXX_coop_play_b { get; set; }
        [JsonPropertyName("match_play_b")]
        public bool XXX_match_play_b { get; set; }
        [JsonPropertyName("ranking_b")]
        public bool XXX_ranking_b { get; set; }
        [JsonPropertyName("reg_only_hidden")]
        public bool XXX_reg_only_hidden { get; set; }
        [JsonPropertyName("price_discounted_f")]
        public float XXX_price_discounted_f { get; set; }
        [JsonPropertyName("indie_b")]
        public bool XXX_indie_b { get; set; }
        [JsonPropertyName("nintendo_switch_online_exclusive_b")]
        public bool XXX_nintendo_switch_online_exclusive_b { get; set; }
        [JsonPropertyName("play_coins")]
        public bool XXX_play_coins { get; set; }
        [JsonPropertyName("download_play")]
        public bool XXX_download_play { get; set; }
        [JsonPropertyName("off_tv_play_b")]
        public bool XXX_off_tv_play_b { get; set; }



#endregion












[JsonExtensionData]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public Dictionary<string, object> _extensionData { get; set; } = new();

[JsonPropertyName("excerpt")]
public string? Excerpt { get; set; }

[JsonPropertyName("fs_id")]
[JsonConverter(typeof(NullableLongJsonConverter))]
public long? FsId { get; set; }
        
[JsonPropertyName("nsuid_txt")]
[JsonConverter(typeof(NsuidListJsonConverter))]
public string? Nsuid { get; set; }

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