using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Red.Core.Application.Json;
using Red.Infrastructure.NintendoApi.Json;

namespace Red.Infrastructure.NintendoApi.Models
{
    [DebuggerDisplay(
        "{Title,nq}",
        Name = "{Nsuid != null ? \"[\" + Nsuid + \"]\" : \"[NSUID UNKNOWN]\",nq}",
        Type = "Nintendo eShop Game")]
    internal sealed class LibrarySearchGame
    {
        [JsonExtensionData]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public ReadOnlyDictionary<string, object> _extensionData { get; init; }
            = new(new Dictionary<string, object>());

        /// <summary>
        ///     Internal eShop version
        /// </summary>
        [JsonPropertyName("_version_")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public long? _version_ { get; init; }

        [JsonPropertyName("add_on_content_b")]
        public bool? AddOnContent { get; init; }

        [JsonPropertyName("add_on_content_b ")]
        public bool? AddOnContentAvailable { get; init; }

        [JsonPropertyName("age_rating_sorting_i")]
        public byte? AgeRating { get; init; }

        [JsonPropertyName("age_rating_type")]
        public string? AgeRatingType { get; init; }

        [JsonPropertyName("age_rating_value")]
        public string? AgeRatingValue { get; init; }

        [JsonPropertyName("change_date")]
        public DateTime? Changed { get; init; }

        [JsonPropertyName("club_nintendo")]
        public bool? ClubNintendo { get; init; }

        [JsonPropertyName("compatible_controller")]
        public List<string>? CompatibleController { get; init; }

        [JsonPropertyName("ranking_b")]
        public bool? CompetitiveRanking { get; init; }

        [JsonPropertyName("coop_play_b")]
        public bool? CoopPlay { get; init; }

        [JsonPropertyName("copyright_s")]
        public string? Copyright { get; init; }

        [JsonPropertyName("date_from")]
        public DateTime? DateFrom { get; init; }

        [JsonPropertyName("demo_availability")]
        public bool? DemoAvailable { get; init; }

        [JsonPropertyName("deprioritise_b")]
        public bool? Deprioritise { get; init; }

        [JsonPropertyName("developer")]
        public string? Developer { get; init; }

        [JsonPropertyName("digital_version_b")]
        public bool? DigitalVersion { get; init; }

        [JsonPropertyName("price_has_discount_b")]
        public bool? Discount { get; init; }

        [JsonPropertyName("price_discount_percentage_f")]
        public float? DiscountPercentage { get; init; }

        [JsonPropertyName("dlc_shown_b")]
        public bool? DlcShown { get; init; }

        [JsonPropertyName("excerpt")]
        public string? Excerpt { get; init; }

        [JsonPropertyName("fs_id")]
        [JsonConverter(typeof(NullableLongJsonConverter))]
        public long? FsId { get; init; }

        // todo: use proper convert (same issue as languages)
        [JsonPropertyName("game_category")]
        public List<string>? GameCategories { get; init; }

        // todo: use proper convert (same issue as languages)
        [JsonPropertyName("game_categories_txt")]
        public List<string>? GameCategoriesTXT { get; init; }

        /// <summary>
        ///     The same as <see cref="GameSeriesT" /> - just as array. No idea why 🤷‍
        /// </summary>
        [JsonPropertyName("game_series_txt")]
        public List<string>? GameSeries { get; init; }

        [JsonPropertyName("game_series_t")]
        public string? GameSeriesT { get; init; }

        [JsonPropertyName("gift_finder_description_s")]
        public string? GiftFinderDescription { get; init; }

        [JsonPropertyName("gift_finder_detail_page_store_link_s")]
        public string? GiftFinderDetailPageStoreLink { get; init; }

        [JsonPropertyName("play_mode_handheld_mode_b")]
        public bool? HandheldMode { get; init; }

        [JsonPropertyName("hd_rumble_b")]
        public bool? HdRumble { get; init; }

        [JsonPropertyName("hits_i")]
        public int? Hits { get; init; }

        [JsonPropertyName("image_url_tm_s")]
        public string? ImageTrademark { get; init; }

        [JsonPropertyName("indie_b")]
        public bool? Indie { get; init; }

        /// <summary>
        ///     Requires internet for some functionality ? not sure
        /// </summary>
        [JsonPropertyName("internet")]
        public bool? Internet { get; init; }

        [JsonPropertyName("ir_motion_camera_b")]
        public bool? IrMotionCamera { get; init; }

        [JsonPropertyName("labo_b")]
        public bool? Labo { get; init; }

        [JsonPropertyName("language_availability")]
        [JsonConverter(typeof(LanguagesJsonConverter))]
        public List<string>? Languages { get; init; }

        [JsonPropertyName("local_play")]
        public bool? LocalPlay { get; init; }

        [JsonPropertyName("price_lowest_f")]
        public decimal? LowestPrice { get; init; }

        [JsonPropertyName("players_to")]
        public byte? MaxPlayers { get; init; }

        [JsonPropertyName("mii_support")]
        public bool? MiiSupport { get; init; }

        [JsonPropertyName("players_from")]
        public byte? MinPlayers { get; init; }

        [JsonPropertyName("multiplayer_mode")]
        public string? MultiplayerMode { get; init; }

        [JsonPropertyName("near_field_comm_b")]
        public bool? NFC { get; init; }

        /// <summary>
        ///     Only available for users with <em>Nintendo Switch Online</em> subscription.
        /// </summary>
        [JsonPropertyName("nintendo_switch_online_exclusive_b")]
        public bool? NintendoSwitchOnlineExclusive { get; init; }

        [JsonPropertyName("originally_for_t")]
        public string? OriginallyFor { get; init; }

        /// <summary>
        ///     PG-Rating (Parental Guidance)
        /// </summary>
        [JsonPropertyName("pg_s")]
        public string? PG { get; init; }

        [JsonPropertyName("physical_version_b")]
        public bool? PhysicalVersion { get; init; }

        /// <summary>
        ///     HAC is Nintendo Switch
        /// </summary>
        [JsonPropertyName("playable_on_txt")]
        public List<string>? PlayableOn { get; init; }

        [JsonPropertyName("popularity")]
        public int? Popularity { get; init; }

        [JsonPropertyName("pretty_agerating_s")]
        public string? PrettyAgeRating { get; init; }

        [JsonPropertyName("pretty_date_s")]
        public string? PrettyDate { get; init; }

        [JsonPropertyName("pretty_game_categories_txt")]
        public List<string>? PrettyGameCategories { get; init; }

        [JsonPropertyName("price_discounted_f")]
        public float? PriceDiscounted { get; init; }

        [JsonPropertyName("price_sorting_f")]
        public float? PriceSorting { get; init; }

        [JsonPropertyName("priority")]
        public DateTime? Priority { get; init; }

        //[JsonPropertyName("product_code_ss")]
        //[JsonConverter(typeof(FirstItemJsonConverter))]
        //[SuppressMessage("ReSharper", "InconsistentNaming")]
        //public string? ProductCodeSS { get; init; }

        //[JsonPropertyName("product_code_txt")]
        //[JsonConverter(typeof(FirstItemJsonConverter))]
        //public string? ProductCodeTXT { get; init; }

        [JsonPropertyName("nsuid_txt")]
        public List<string>? Nsuids { get; init; }

        [JsonPropertyName("product_code_ss")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public List<string>? ProductCodeSS { get; init; }

        [JsonPropertyName("product_code_txt")]
        public List<string>? ProductCodeTXT { get; init; }




        [JsonPropertyName("publisher")]
        public string? Publisher { get; init; }

        [JsonPropertyName("price_regular_f")]
        public float? RegularPrice { get; init; }

        [JsonPropertyName("dates_released_dts")]
        [JsonConverter(typeof(ReleaseDateJsonConverter))]
        public DateTime? ReleaseDate { get; init; }

        [JsonPropertyName("eshop_removed_b")]
        public bool? RemovedFromEshop { get; init; }

        [JsonPropertyName("sorting_title")]
        public string? SortingTitle { get; init; }

        [JsonPropertyName("paid_subscription_required_b")]
        public bool? SubscriptionRequired { get; init; }

        [JsonPropertyName("cloud_saves_b")]
        public bool? SupportsCloudSave { get; init; }

        [JsonPropertyName("switch_game_voucher_b")]
        public bool? SwitchGameVoucher { get; init; }

        [JsonPropertyName("system_names_txt")]
        public List<string>? SystemNames { get; init; }

        [JsonPropertyName("system_type")]
        [JsonConverter(typeof(FirstItemJsonConverter))] // todo: wrong converter bro.. should use same as languages i believe
        public string? SystemType { get; init; }

        [JsonPropertyName("play_mode_tabletop_mode_b")]
        public bool? TabletopMode { get; init; }

        [JsonPropertyName("title")]
        public string? Title { get; init; }

        [JsonPropertyName("title_extras_txt")]
        public List<string>? TitleExtras { get; init; }

        [JsonPropertyName("play_mode_tv_mode_b")]
        public bool? TvMode { get; init; }

        [JsonPropertyName("type")]
        public string? Type { get; init; }

        [JsonPropertyName("url")]
        public string? Url { get; init; }

        [JsonPropertyName("voice_chat_b")]
        public bool? VoiceChat { get; init; }

        #region unknown

        // ReSharper disable InconsistentNaming
        [JsonPropertyName("reg_only_hidden")]
        public bool? reg_only_hidden { get; init; }

        [JsonPropertyName("match_play_b")]
        public bool? match_play_b { get; init; }
        // ReSharper restore InconsistentNaming

        #endregion

        #region Non-Switch

        /// <summary>
        ///     Nintendo 3DS Feature.
        /// </summary>
        /// <remarks>https://nintendo.fandom.com/wiki/Play_Coins</remarks>
        [JsonPropertyName("play_coins")]
        public bool? PlayCoins { get; init; }

        /// <summary>
        ///     Nintendo 2DS / 3DS Feature. <br />
        ///     Download Play allows multiple systems to connect for a multiplayer game session.
        /// </summary>
        [JsonPropertyName("download_play")]
        public bool? DownloadPlay { get; init; }

        /// <summary>
        ///     Wii U Feature.
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Off-TV_Play</remarks>
        [JsonPropertyName("off_tv_play_b")]
        public bool? OffTvPlay { get; init; }

        #endregion

        // ReSharper disable InconsistentNaming
        public string? image_url { get; init; } // ImageSquare
        public string? image_url_sq_s { get; init; }
        public string? image_url_h2x1_s { get; init; }
        public string? wishlist_email_square_image_url_s { get; init; }
        public string? wishlist_email_banner640w_image_url_s { get; init; } // ImageWide
        public string? wishlist_email_banner460w_image_url_s { get; init; }
        public string? gift_finder_wishlist_image_url_s { get; init; }
        public string? gift_finder_detail_page_image_url_s { get; init; }

        public string? gift_finder_carousel_image_url_s { get; init; }
        // ReSharper restore InconsistentNaming
    }
}