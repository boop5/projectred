using System;
using System.Collections.Generic;
using System.Text;
using EzNintendo.Common.Extensions.System;
using EzNintendo.Domain.Converter;
using Newtonsoft.Json;

namespace EzNintendo.Domain.Nintendo
{
    public sealed class GameDTO
    {
        private string _ageRatingType;
        private string _ageRatingValue;
        private string _developer;
        private string _excerpt;
        private string _publisher;
        private string _sortingTitle;
        private string _title;
        private string _type;

        [JsonProperty("price_regular_f")]
        public float? RegularPrice { get; set; }
        
        [JsonProperty("title")]
        public string Title
        {
            get => _title;
            set => _title = value ?? string.Empty;
        }

        // [JsonProperty("fs_id")] public long FsId { get; set; }

        [JsonProperty("nsuid_txt")]
        [JsonConverter(typeof(NsuidListConverter))]
        public NsuId NsUid { get; set; }

        [JsonProperty("dates_released_dts")]
        [JsonConverter(typeof(ReleaseDateConverter))]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("publisher")]
        public string Publisher
        {
            get => _publisher;
            set => _publisher = value ?? string.Empty;
        }

        [JsonProperty("excerpt")]
        public string Excerpt
        {
            get => _excerpt;
            set => _excerpt = value ?? string.Empty;
        }

        //[JsonProperty("image_url")]
        //[JsonConverter(typeof(NintendoImageUrlConverter))]
        //public string? ImageSquare { get; set; }

        //[JsonProperty("wishlist_email_banner640w_image_url_s")]
        //[JsonConverter(typeof(NintendoImageUrlConverter))]
        //public string? ImageWide { get; set; }

        #region notsoImportantLol

        [JsonProperty("add_on_content_b ")]
        public bool? AddOnContentAvailable { get; set; }

        [JsonProperty("change_date")]
        public DateTime? Changed { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("type")]
        public string Type
        {
            get => _type;
            set => _type = value ?? string.Empty;
        }

        [JsonProperty("club_nintendo")]
        public bool? ClubNintendo { get; set; }

        [JsonProperty("hd_rumble_b")]
        public bool? HdRumble { get; set; }

        [JsonProperty("multiplayer_mode")]
        [JsonConverter(typeof(MultiplayerModeConverter))]
        public MultiplayerMode? MultiplayerMode { get; set; }

        [JsonProperty("play_mode_tv_mode_b")]
        public bool? TvMode { get; set; }

        [JsonProperty("play_mode_handheld_mode_b")]
        public bool? HandheldMode { get; set; }

        [JsonProperty("product_code_txt")]
        [JsonConverter(typeof(ListToItemConverter<string>))]
        public string? ProductCode { get; set; }

        [JsonProperty("compatible_controller")]
        [JsonConverter(typeof(CompatibleControllerConverter))]
        public List<string>? CompatibleController { get; set; }

        [JsonProperty("paid_subscription_required_b")]
        public bool? SubscriptionRequired { get; set; }

        [JsonProperty("cloud_saves_b")]
        public bool? SupportsCloudSave { get; set; }

        [JsonProperty("age_rating_sorting_i")]
        public byte? AgeRating { get; set; }

        [JsonProperty("age_rating_type")]
        public string AgeRatingType
        {
            get => _ageRatingType;
            set => _ageRatingType = value ?? string.Empty;
        }

        [JsonProperty("age_rating_value")]
        public string AgeRatingValue
        {
            get => _ageRatingValue;
            set => _ageRatingValue = value ?? string.Empty;
        }

        [JsonProperty("play_mode_tabletop_mode_b")]
        public bool? TabletopMode { get; set; }

        [JsonProperty("language_availability")]
        [JsonConverter(typeof(LanguagesConverter))]
        public List<string>? Languages { get; set; }

        [JsonProperty("price_has_discount_b")]
        public bool? Discount { get; set; }

        [JsonProperty("price_discount_percentage_f")]
        public float? DiscountPercentage { get; set; }

        [JsonProperty("sorting_title")]
        public string SortingTitle
        {
            get => _sortingTitle;
            set => _sortingTitle = value ?? string.Empty;
        }

        [JsonProperty("voice_chat_b")]
        public bool? VoiceChat { get; set; }

        [JsonProperty("developer", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Developer
        {
            get => _developer;
            set => _developer = value ?? string.Empty;
        }

        [JsonProperty("physical_version_b")]
        public bool? PhysicalVersion { get; set; }

        [JsonProperty("digital_version_b")]
        public bool? DigitalVersion { get; set; }

        [JsonProperty("price_lowest_f")]
        [JsonConverter(typeof(StringToDecimalConverter))]
        public decimal? LowestPrice { get; set; }

        [JsonProperty("players_from")]
        public byte? MinPlayers { get; set; }

        [JsonProperty("players_to")]
        [JsonConverter(typeof(StringToByteConverter))]
        public byte? MaxPlayers { get; set; }

        [JsonProperty("pretty_game_categories_txt")]
        public List<string>? Categories { get; set; }

        /*
         * todo:
         *
         * ImageSquare => image_url_sq_s
         * ImageWide => passt ✅
         * ImageCover => gift_finder_detail_page_image_url_s (nur wenn PhysicalVersion == true)
         *
         * todo: upload als    <bucket>/<fsid>/wide.jpg
         *                     <bucket>/<fsid>/square.jpg
         *                     <bucket>/<fsid>/cover.jpg
         *
         * todo: check if there is a folder with less than 8 pictures in E:\source\EzNintendo\data\pics
         */

        [JsonConverter(typeof(NintendoImageUrlConverter))] 
        public string image_url                                  {get;set; } // ImageSquare
        [JsonConverter(typeof(NintendoImageUrlConverter))]
        public string? image_url_sq_s { get; set; }

        [JsonConverter(typeof(NintendoImageUrlConverter))]
        public string? image_url_h2x1_s { get; set; }

        [JsonConverter(typeof(NintendoImageUrlConverter))]
        public string? wishlist_email_square_image_url_s { get; set; }

        //[JsonConverter(typeof(NintendoImageUrlConverter))] public string wishlist_email_banner640w_image_url_s      {get;set;} // ImageWide
        [JsonConverter(typeof(NintendoImageUrlConverter))]
        public string? wishlist_email_banner460w_image_url_s { get; set; }

        [JsonConverter(typeof(NintendoImageUrlConverter))]
        public string? gift_finder_wishlist_image_url_s { get; set; }

        [JsonConverter(typeof(NintendoImageUrlConverter))]
        public string? gift_finder_detail_page_image_url_s { get; set; }


        #endregion

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var (k, v) in this.DictionaryFromType())
            {
                builder.AppendFormat("{0}: {1}; ", k, v);
            }

            return builder.ToString();
        }
    }
}