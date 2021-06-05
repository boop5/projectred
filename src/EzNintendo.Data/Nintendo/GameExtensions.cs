using EzNintendo.Domain;
using EzNintendo.Domain.Nintendo;

namespace EzNintendo.Data.Nintendo
{
    public static class GameExtensions
    {
        public static void UpdateFrom(this Game dbGame, GameDTO game)
        {
            dbGame.AddOnContentAvailable = game.AddOnContentAvailable;
            dbGame.AgeRating = game.AgeRating;
            dbGame.AgeRatingType = game.AgeRatingType;
            dbGame.ClubNintendo = game.ClubNintendo;
            dbGame.Developer = game.Developer;
            dbGame.DigitalVersion = game.DigitalVersion;
            dbGame.Excerpt = game.Excerpt;
            dbGame.HandheldMode = game.HandheldMode;
            dbGame.HdRumble = game.HdRumble;
            dbGame.MaxPlayers = game.MaxPlayers;
            dbGame.MinPlayers = game.MinPlayers;
            dbGame.MultiplayerMode = game.MultiplayerMode;
            dbGame.PhysicalVersion = game.PhysicalVersion;
            dbGame.Publisher = game.Publisher;
            dbGame.ReleaseDate = game.ReleaseDate;
            dbGame.SortingTitle = game.SortingTitle;
            dbGame.SubscriptionRequired = game.SubscriptionRequired;
            dbGame.SupportsCloudSave = game.SupportsCloudSave;
            dbGame.TabletopMode = game.TabletopMode;
            dbGame.Title = game.Title;
            dbGame.TvMode = game.TvMode;
            dbGame.VoiceChat = game.VoiceChat;
            dbGame.image = game.image_url;
            dbGame.image_giftfinder_detailpage = game.gift_finder_detail_page_image_url_s;
            dbGame.image_giftfinder_wishlist = game.gift_finder_wishlist_image_url_s;
            dbGame.image_h2x1 = game.image_url_h2x1_s;
            dbGame.image_sq = game.image_url_sq_s;
            dbGame.image_wishlist_email_banner640w = game.wishlist_email_banner460w_image_url_s;
            dbGame.image_wishlist_email_square = game.wishlist_email_square_image_url_s;
        }
    }
}