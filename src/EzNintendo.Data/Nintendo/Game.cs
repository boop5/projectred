using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using EzNintendo.Data.Base;
using EzNintendo.Data.Converter;
using EzNintendo.Domain;
using EzNintendo.Domain.Nintendo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EzNintendo.Data.Nintendo
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [DebuggerDisplay("{Title,nq}", Name = "wtfIsName")]
    [Table(nameof(Game), Schema = "Nintendo")]
    public sealed class Game : IIdentifiableRecord, ITrackCreated, ITrackUpdated
    {
        public NsuId NsuidByRegion(string region)
        {
            if (region.ToLower() == "europe")  return NsUid_EU;
            if (region.ToLower() == "united states")  return NsUid_US;
            if (region.ToLower() == "japan")  return NsUid_JP;

            throw new ArgumentOutOfRangeException();
        }

        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        //[Required] public long FsId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? SortingTitle { get; set; }

        [Column(TypeName = "nvarchar(2000)")]
        public string? Excerpt { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Publisher { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Developer { get; set; }

        public bool? HdRumble { get; set; }
        public bool? AddOnContentAvailable { get; set; }
        public bool? TvMode { get; set; }
        public bool? SubscriptionRequired { get; set; }
        public bool? ClubNintendo { get; set; } // todo: wtf ist das
        public bool? SupportsCloudSave { get; set; }
        public bool? VoiceChat { get; set; }
        public bool? DigitalVersion { get; set; }
        public bool? PhysicalVersion { get; set; }

        public byte? AgeRating { get; set; }

        [Column(TypeName = "nvarchar(25)")]
        public string? AgeRatingType { get; set; }

        public byte? MinPlayers { get; set; }
        public byte? MaxPlayers { get; set; }

        [Column(TypeName = "nvarchar(12)")]
        public MultiplayerMode? MultiplayerMode { get; set; }

        public NsuId? NsUid_EU { get; set; }
        public NsuId? NsUid_JP { get; set; }
        public NsuId? NsUid_US { get; set; }

        public bool? ProController { get; set; }
        public bool? TabletopMode { get; set; }
        public bool? HandheldMode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReleaseDate { get; set; }

        public ICollection<GameLanguage> GameLanguages { get; set; }
        public ICollection<GameCategory> GameCategories { get; set; }
        public ICollection<Trend> Trend { get; set; }

        public string? image_giftfinder_detailpage { get; set; }
        public string? image_giftfinder_wishlist { get; set; }
        public string? image { get; set; } // ImageSquare
        public string? image_h2x1 { get; set; }
        public string? image_sq { get; set; }
        public string? image_wishlist_email_banner640w { get; set; } // ImageWide
        public string? image_wishlist_email_square { get; set; }

        public static void Setup(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                        .Property(g => g.MultiplayerMode)
                        .HasConversion(new EnumToStringConverter<MultiplayerMode>());

            modelBuilder.Entity<Game>().Property(g => g.NsUid_EU).HasConversion(new NsuidToLongConverter());
            modelBuilder.Entity<Game>().Property(g => g.NsUid_JP).HasConversion(new NsuidToLongConverter());
            modelBuilder.Entity<Game>().Property(g => g.NsUid_US).HasConversion(new NsuidToLongConverter());

            modelBuilder.Entity<Game>()
                        .HasMany(g => g.Trend)
                        .WithOne(s => s.Game);

            modelBuilder.Entity<Game>().HasIndex(g => g.Title);
            modelBuilder.Entity<Game>().HasIndex(g => g.Developer);
            modelBuilder.Entity<Game>().HasIndex(g => g.Publisher);
            modelBuilder.Entity<Game>().HasIndex(g => g.ReleaseDate);
        }

        public static bool Validate(Game game)
        {
            return Validate(game, out _);
        }

        public static bool Validate(Game game, out string reason)
        {
            if (game == null)
            {
                reason = "Game is null";
            }
            //else if (game.Id == default)
            //{
            //    reason = "Id is null";
            //}
            //else if (game.FsId == default)
            //{
            //    reason = "FsId is null";
            //}
            else if (game.Title == default)
            {
                reason = "Title is null";
            }
            else
            {
                reason = string.Empty;
            }

            return string.IsNullOrEmpty(reason);
        }

        public static Game FromDTO(GameDTO dto)
        {
            var dbGame = new Game();
            dbGame.UpdateFrom(dto);
            return dbGame;


            //var dbGame = new Game
            //{
            //    AddOnContentAvailable = dto.AddOnContentAvailable,
            //    AgeRating = dto.AgeRating,
            //    AgeRatingType = dto.AgeRatingType,
            //    ClubNintendo = dto.ClubNintendo,
            //    Developer = dto.Developer,
            //    DigitalVersion = dto.DigitalVersion,
            //    Excerpt = dto.Excerpt,
            //    HandheldMode = dto.HandheldMode,
            //    HdRumble = dto.HdRumble,
            //    MaxPlayers = dto.MaxPlayers,
            //    MinPlayers = dto.MinPlayers,
            //    MultiplayerMode = dto.MultiplayerMode,
            //    PhysicalVersion = dto.PhysicalVersion,
            //    Publisher = dto.Publisher,
            //    ReleaseDate = dto.ReleaseDate,
            //    SortingTitle = dto.SortingTitle,
            //    SubscriptionRequired = dto.SubscriptionRequired,
            //    SupportsCloudSave = dto.SupportsCloudSave,
            //    TabletopMode = dto.TabletopMode,
            //    Title = dto.Title,
            //    TvMode = dto.TvMode,
            //    VoiceChat = dto.VoiceChat,

            //    image = dto.image_url,
            //    image_giftfinder_detailpage =      dto.gift_finder_detail_page_image_url_s,
            //    image_giftfinder_wishlist =        dto.gift_finder_wishlist_image_url_s,
            //    image_h2x1 =                       dto.image_url_h2x1_s,
            //    image_sq =                         dto.image_url_sq_s,
            //    image_wishlist_email_banner640w =  dto.wishlist_email_banner460w_image_url_s,
            //    image_wishlist_email_square =      dto.wishlist_email_square_image_url_s
            //};

            //return dbGame;
        }

        public bool Equals(Game a, Game b, out string reason)
        {
            var stringComparison = StringComparison.Ordinal;

            if (!Equals(a.AddOnContentAvailable, b.AddOnContentAvailable))
            {
                reason = $"{nameof(AddOnContentAvailable)} is different. (a: {a.AddOnContentAvailable}, b: {b.AddOnContentAvailable})";

                return false;
            }

            if (!Equals(a.AgeRating, b.AgeRating))
            {
                reason = $"{nameof(AgeRating)} is different (a: {a.AgeRating}; b: {b.AgeRating})";

                return false;
            }

            if (!string.Equals(a.AgeRatingType, b.AgeRatingType, stringComparison))
            {
                reason = $"{nameof(AgeRatingType)} is different (a: {a.AgeRatingType}; b: {b.AgeRatingType})";

                return false;
            }

            if (!Equals(a.ClubNintendo, b.ClubNintendo))
            {
                reason = $"{nameof(ClubNintendo)} is different (a: {a.ClubNintendo}; b: {b.ClubNintendo})";

                return false;
            }

            if (!string.Equals(a.Developer, b.Developer, stringComparison))
            {
                reason = $"{nameof(Developer)} is different (a: {a.Developer}; b: {b.Developer})";

                return false;
            }

            if (!Equals(a.DigitalVersion, b.DigitalVersion))
            {
                reason = $"{nameof(DigitalVersion)} is different (a: {a.DigitalVersion}; b: {b.DigitalVersion})";

                return false;
            }

            if (!string.Equals(a.Excerpt, b.Excerpt, stringComparison))
            {
                reason = $"{nameof(Excerpt)} is different (a: {a.Excerpt}; b: {b.Excerpt})";

                return false;
            }

            if (!Equals(a.HandheldMode, b.HandheldMode))
            {
                reason = $"{nameof(HandheldMode)} is different (a: {a.HandheldMode}; b: {b.HandheldMode})";

                return false;
            }

            if (!Equals(a.HdRumble, b.HdRumble))
            {
                reason = $"{nameof(HdRumble)} is different (a: {a.HdRumble}; b: {b.HdRumble})";

                return false;
            }

            if (!Equals(a.MaxPlayers, b.MaxPlayers))
            {
                reason = $"{nameof(MaxPlayers)} is different (a: {a.MaxPlayers}; b: {b.MaxPlayers})";

                return false;
            }

            if (!Equals(a.MinPlayers, b.MinPlayers))
            {
                reason = $"{nameof(MinPlayers)} is different (a: {a.MinPlayers}; b: {b.MinPlayers})";

                return false;
            }

            if (!Equals(a.PhysicalVersion, b.PhysicalVersion))
            {
                reason = $"{nameof(PhysicalVersion)} is different (a: {a.PhysicalVersion}; b: {b.PhysicalVersion})";

                return false;
            }

            if (!string.Equals(a.Publisher, b.Publisher, stringComparison))
            {
                reason = $"{nameof(Publisher)} is different (a: {a.Publisher}; b: {b.Publisher})";

                return false;
            }

            // todo: a.ReleaseDate.Date ???
            if (!Equals(a.ReleaseDate, b.ReleaseDate))
            {
                reason = $"{nameof(ReleaseDate)} is different (a: {a.ReleaseDate}; b: {b.ReleaseDate})";

                return false;
            }

            if (!string.Equals(a.SortingTitle, b.SortingTitle, stringComparison))
            {
                reason = $"{nameof(SortingTitle)} is different (a: {a.SortingTitle}; b: {b.SortingTitle})";

                return false;
            }

            if (!Equals(a.SubscriptionRequired, b.SubscriptionRequired))
            {
                reason = $"{nameof(SubscriptionRequired)} is different (a: {a.SubscriptionRequired}; b: {b.SubscriptionRequired})";

                return false;
            }

            if (!Equals(a.SupportsCloudSave, b.SupportsCloudSave))
            {
                reason = $"{nameof(SupportsCloudSave)} is different (a: {a.SupportsCloudSave}; b: {b.SupportsCloudSave})";

                return false;
            }

            if (!Equals(a.TabletopMode, b.TabletopMode))
            {
                reason = $"{nameof(TabletopMode)} is different (a: {a.TabletopMode}; b: {b.TabletopMode})";

                return false;
            }

            if (!string.Equals(a.Title, b.Title, stringComparison))
            {
                reason = $"{nameof(Title)} is different (a: {a.Title}; b: {b.Title})";

                return false;
            }

            if (!Equals(a.TvMode, b.TvMode))
            {
                reason = $"{nameof(TvMode)} is different (a: {a.TvMode}; b: {b.TvMode})";

                return false;
            }

            if (!Equals(a.VoiceChat, b.VoiceChat))
            {
                reason = $"{nameof(VoiceChat)} is different (a: {a.VoiceChat}; b: {b.VoiceChat})";

                return false;
            }

            if (!Equals(a.MultiplayerMode, b.MultiplayerMode))
            {
                reason = $"{nameof(MultiplayerMode)} is different (a: {a.MultiplayerMode}; b: {b.MultiplayerMode})";

                return false;
            }

            if (!Equals(a.image, b.image))
            {
                reason = $"{nameof(image)} is different (a: {a.image}; b: {b.image})";

                return false;
            }

            if (!Equals(a.image_giftfinder_detailpage, b.image_giftfinder_detailpage))
            {
                reason = $"{nameof(image_giftfinder_detailpage)} is different (a: {a.image_giftfinder_detailpage}; b: {b.image_giftfinder_detailpage})";

                return false;
            }

            if (!Equals(a.image_giftfinder_wishlist, b.image_giftfinder_wishlist))
            {
                reason = $"{nameof(image_giftfinder_wishlist)} is different (a: {a.image_giftfinder_wishlist}; b: {b.image_giftfinder_wishlist})";

                return false;
            }

            if (!Equals(a.image_h2x1, b.image_h2x1))
            {
                reason = $"{nameof(image_h2x1)} is different (a: {a.image_h2x1}; b: {b.image_h2x1})";

                return false;
            }

            if (!Equals(a.image_sq, b.image_sq))
            {
                reason = $"{nameof(image_sq)} is different (a: {a.image_sq}; b: {b.image_sq})";

                return false;
            }

            if (!Equals(a.image_wishlist_email_banner640w, b.image_wishlist_email_banner640w))
            {
                reason = $"{nameof(image_wishlist_email_banner640w)} is different (a: {a.image_wishlist_email_banner640w}; b: {b.image_wishlist_email_banner640w})";

                return false;
            }

            if (!Equals(a.image_wishlist_email_square, b.image_wishlist_email_square))
            {
                reason = $"{nameof(image_wishlist_email_square)} is different (a: {a.image_wishlist_email_square}; b: {b.image_wishlist_email_square})";

                return false;
            }

            reason = string.Empty;
            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj, out _);
        }

        public bool Equals(object obj, out string reason)
        {
            reason = string.Empty;

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is Game other)
            {
                return Equals(this, other, out reason);
            }

            return false;
        }

        public bool Equals(GameDTO a, out string reason)
        {
            var dbGame = FromDTO(a);

            return Equals(dbGame, out reason);
        }

        public bool Equals(GameDTO b)
        {
            var bb = FromDTO(b);

            return Equals(bb);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode", Justification = "They need to have a setter for EFCore.")]
        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ Created.GetHashCode();
            }
        }

        public string GetNormalizedTitle(IFileSystem fs = null)
        {
            // todo: crap. make this better. like an hash or shit
            if (fs == null)
            {
                fs = new FileSystem();
            }

            var title = (string) Title.Clone();

            foreach (var c in fs.Path.GetInvalidFileNameChars())
            {
                title = title.Replace(c, '_');
            }

            return title;
        }
    }
}