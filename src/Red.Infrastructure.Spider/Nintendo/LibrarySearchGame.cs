using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;
using Red.Infrastructure.Spider.Json;

namespace Red.Infrastructure.Spider.Nintendo
{
    [DebuggerDisplay(
        "{Title,nq}",
        Name = "{Nsuid != null ? \"[\" + Nsuid + \"]\" : \"[NSUID UNKNOWN]\",nq}", 
        Type = "Nintendo eShop Game")]
    public sealed class LibrarySearchGame
    {
        [JsonExtensionData]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public Dictionary<string, object> _extensionData { get; set; } = new();

        /// <summary>
        ///     Internal eShop version
        /// </summary>
        [JsonPropertyName("_version_")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long? _version_ { get; set; }

        [JsonPropertyName("add_on_content_b")]
        public bool? AddOnContent { get; set; }

        [JsonPropertyName("add_on_content_b ")]
        public bool? AddOnContentAvailable { get; set; }

        [JsonPropertyName("age_rating_sorting_i")]
        public byte? AgeRating { get; set; }

        [JsonPropertyName("age_rating_type")]
        public string? AgeRatingType { get; set; }

        [JsonPropertyName("age_rating_value")]
        public string? AgeRatingValue { get; set; }

        [JsonPropertyName("pretty_game_categories_txt")]
        public List<string>? Categories { get; set; }

        [JsonPropertyName("change_date")]
        public DateTime? Changed { get; set; }

        [JsonPropertyName("club_nintendo")]
        public bool? ClubNintendo { get; set; }

        [JsonPropertyName("compatible_controller")]
        public List<string>? CompatibleController { get; set; }

        [JsonPropertyName("ranking_b")]
        public bool? CompetitiveRanking { get; set; }

        [JsonPropertyName("coop_play_b")]
        public bool? CoopPlay { get; set; }

        [JsonPropertyName("copyright_s")]
        public string? Copyright { get; set; }

        [JsonPropertyName("date_from")]
        public DateTime? DateFrom { get; set; }

        [JsonPropertyName("demo_availability")]
        public bool? DemoAvailable { get; set; }

        [JsonPropertyName("deprioritise_b")]
        public bool? Deprioritise { get; set; }

        [JsonPropertyName("developer")]
        public string? Developer { get; set; }

        [JsonPropertyName("digital_version_b")]
        public bool? DigitalVersion { get; set; }

        [JsonPropertyName("price_has_discount_b")]
        public bool? Discount { get; set; }

        [JsonPropertyName("price_discount_percentage_f")]
        public float? DiscountPercentage { get; set; }

        [JsonPropertyName("dlc_shown_b")]
        public bool? DlcShown { get; set; }

        [JsonPropertyName("excerpt")]
        public string? Excerpt { get; set; }

        [JsonPropertyName("fs_id")]
        [JsonConverter(typeof(NullableLongJsonConverter))]
        public long? FsId { get; set; }

        [JsonPropertyName("game_category")]
        public List<string>? GameCategories { get; set; }

        [JsonPropertyName("game_categories_txt")]
        public List<string>? GameCategoriesTXT { get; set; }

        /// <summary>
        ///     The same as <see cref="GameSeriesT" /> - just as array. No idea why 🤷‍
        /// </summary>
        [JsonPropertyName("game_series_txt")]
        public List<string>? GameSeries { get; set; }

        [JsonPropertyName("game_series_t")]
        public string? GameSeriesT { get; set; }

        [JsonPropertyName("gift_finder_description_s")]
        public string? GiftFinderDescription { get; set; }

        [JsonPropertyName("gift_finder_detail_page_store_link_s")]
        public string? GiftFinderDetailPageStoreLink { get; set; }

        [JsonPropertyName("play_mode_handheld_mode_b")]
        public bool? HandheldMode { get; set; }

        [JsonPropertyName("hd_rumble_b")]
        public bool? HdRumble { get; set; }

        [JsonPropertyName("hits_i")]
        public int? Hits { get; set; }

        [JsonPropertyName("image_url_tm_s")]
        public string? ImageTrademark { get; set; }

        [JsonPropertyName("indie_b")]
        public bool? Indie { get; set; }

        /// <summary>
        ///     Requires internet for some functionality ? not sure
        /// </summary>
        [JsonPropertyName("internet")]
        public bool? Internet { get; set; }

        [JsonPropertyName("ir_motion_camera_b")]
        public bool? IrMotionCamera { get; set; }

        [JsonPropertyName("labo_b")]
        public bool? Labo { get; set; }

        [JsonPropertyName("language_availability")]
        [JsonConverter(typeof(LanguagesJsonConverter))]
        public List<string>? Languages { get; set; }

        [JsonPropertyName("local_play")]
        public bool? LocalPlay { get; set; }

        [JsonPropertyName("price_lowest_f")]
        public decimal? LowestPrice { get; set; }

        [JsonPropertyName("players_to")]
        public byte? MaxPlayers { get; set; }

        [JsonPropertyName("mii_support")]
        public bool? MiiSupport { get; set; }

        [JsonPropertyName("players_from")]
        public byte? MinPlayers { get; set; }

        [JsonPropertyName("multiplayer_mode")]
        public string? MultiplayerMode { get; set; }

        [JsonPropertyName("near_field_comm_b")]
        public bool? NFC { get; set; }

        /// <summary>
        ///     Only available for users with <em>Nintendo Switch Online</em> subscription.
        /// </summary>
        [JsonPropertyName("nintendo_switch_online_exclusive_b")]
        public bool? NintendoSwitchOnlineExclusive { get; set; }

        [JsonPropertyName("nsuid_txt")]
        [JsonConverter(typeof(NsuidListJsonConverter))]
        public string? Nsuid { get; set; }

        [JsonPropertyName("originally_for_t")]
        public string? OriginallyFor { get; set; }

        /// <summary>
        ///     PG-Rating (Parental Guidance)
        /// </summary>
        [JsonPropertyName("pg_s")]
        public string? PG { get; set; }

        [JsonPropertyName("physical_version_b")]
        public bool? PhysicalVersion { get; set; }

        /// <summary>
        ///     HAC is Nintendo Switch
        /// </summary>
        [JsonPropertyName("playable_on_txt")]
        public List<string>? PlayableOn { get; set; }

        [JsonPropertyName("popularity")]
        public int? Popularity { get; set; }

        [JsonPropertyName("pretty_agerating_s")]
        public string? PrettyAgeRating { get; set; }

        [JsonPropertyName("pretty_date_s")]
        public string? PrettyDate { get; set; }

        [JsonPropertyName("price_discounted_f")]
        public float? PriceDiscounted { get; set; }

        [JsonPropertyName("price_sorting_f")]
        public float? PriceSorting { get; set; }

        [JsonPropertyName("priority")]
        public string? Priority { get; set; }

        [JsonPropertyName("product_code_ss")]
        [JsonConverter(typeof(FirstItemJsonConverter))]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public string? ProductCodeSS { get; set; }

        [JsonPropertyName("product_code_txt")]
        [JsonConverter(typeof(FirstItemJsonConverter))]
        public string? ProductCodeTXT { get; set; }

        [JsonPropertyName("publisher")]
        public string? Publisher { get; set; }

        [JsonPropertyName("price_regular_f")]
        public float? RegularPrice { get; set; }

        [JsonPropertyName("dates_released_dts")]
        [JsonConverter(typeof(ReleaseDateJsonConverter))]
        public DateTime? ReleaseDate { get; set; }

        [JsonPropertyName("eshop_removed_b")]
        public bool? RemovedFromEshop { get; set; }

        [JsonPropertyName("sorting_title")]
        public string? SortingTitle { get; set; }

        [JsonPropertyName("paid_subscription_required_b")]
        public bool? SubscriptionRequired { get; set; }

        [JsonPropertyName("cloud_saves_b")]
        public bool? SupportsCloudSave { get; set; }

        [JsonPropertyName("switch_game_voucher_b")]
        public bool? SwitchGameVoucher { get; set; }

        [JsonPropertyName("system_names_txt")]
        public List<string>? SystemNames { get; set; }

        [JsonPropertyName("system_type")]
        [JsonConverter(typeof(FirstItemJsonConverter))]
        public string? SystemType { get; set; }

        [JsonPropertyName("play_mode_tabletop_mode_b")]
        public bool? TabletopMode { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("title_extras_txt")]
        public List<string>? TitleExtras { get; set; }

        [JsonPropertyName("play_mode_tv_mode_b")]
        public bool? TvMode { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("voice_chat_b")]
        public bool? VoiceChat { get; set; }

        #region unknown

        [JsonPropertyName("reg_only_hidden")]
        public bool? reg_only_hidden { get; set; }

        [JsonPropertyName("match_play_b")]
        public bool? match_play_b { get; set; }

        #endregion

        #region Non-Switch

        /// <summary>
        ///     Nintendo 3DS Feature.
        /// </summary>
        /// <remarks>https://nintendo.fandom.com/wiki/Play_Coins</remarks>
        [JsonPropertyName("play_coins")]
        public bool? PlayCoins { get; set; }

        /// <summary>
        ///     Nintendo 2DS / 3DS Feature. <br />
        ///     Download Play allows multiple systems to connect for a multiplayer game session.
        /// </summary>
        [JsonPropertyName("download_play")]
        public bool? DownloadPlay { get; set; }

        /// <summary>
        ///     Wii U Feature.
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Off-TV_Play</remarks>
        [JsonPropertyName("off_tv_play_b")]
        public bool? OffTvPlay { get; set; }

        #endregion

        // ReSharper disable InconsistentNaming
        public string? image_url { get; set; } // ImageSquare
        public string? image_url_sq_s { get; set; }
        public string? image_url_h2x1_s { get; set; }
        public string? wishlist_email_square_image_url_s { get; set; }
        public string? wishlist_email_banner640w_image_url_s { get; set; } // ImageWide
        public string? wishlist_email_banner460w_image_url_s { get; set; }
        public string? gift_finder_wishlist_image_url_s { get; set; }
        public string? gift_finder_detail_page_image_url_s { get; set; }
        public string? gift_finder_carousel_image_url_s { get; set; }
        // ReSharper restore InconsistentNaming
    }
}