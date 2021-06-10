using System;
using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGame
    {
        #region Required / Has default value

        public string ProductCode { get; init; } = "";
        public string Region { get; init; } = "";
        public SwitchGamePictures Pictures { get; init; } = new();

        /// <summary>Meant to use to sort search results.</summary>
        /// <remarks>Higher numbers mean less popularity.</remarks>
        public int Popularity { get; init; } = int.MaxValue;

        #endregion

        #region Pricing (might move to its own table?)

        public List<PriceRecord>? PriceHistory { get; init; }
        public float? RegularPrice { get; init; }
        public float? AllTimeLow { get; init; }
        public float? AllTimeHigh { get; init; }

        #endregion

        #region Optional

        public string? Slug { get; init; }
        public List<string>? Nsuids { get; init; }
        public string? Title { get; init; }
        public string? Description { get; init; }
        public string? Publisher { get; init; }
        public string? Developer { get; init; }
        public DateTime? ReleaseDate { get; init; }
        public List<string>? Categories { get; init; }
        public int? AgeRating { get; init; }
        public int? DownloadSize { get; init; }
        public int? MinPlayers { get; init; }
        public int? MaxPlayers { get; init; }
        public bool? Coop { get; init; }
        public bool? DemoAvailable { get; init; }
        public List<string>? Languages { get; init; }
        public List<string>? PlayModes { get; init; }
        public bool? SupportsCloudSave { get; init; }
        public bool? RemovedFromEshop { get; init; }
        public bool? VoucherPossible { get; init; }

        #endregion

        #region Equality

        public bool Equals(SwitchGame? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Nullable.Equals(RegularPrice, other.RegularPrice)
                   && Nullable.Equals(AllTimeLow, other.AllTimeLow)
                   && Nullable.Equals(AllTimeHigh, other.AllTimeHigh)
                   && Nullable.Equals(ReleaseDate, other.ReleaseDate)
                   && Title == other.Title
                   && Slug == other.Slug
                   && Description == other.Description
                   && Publisher == other.Publisher
                   && Developer == other.Developer
                   && AgeRating == other.AgeRating
                   && DownloadSize == other.DownloadSize
                   && MinPlayers == other.MinPlayers
                   && MaxPlayers == other.MaxPlayers
                   && Coop == other.Coop
                   && DemoAvailable == other.DemoAvailable
                   && SupportsCloudSave == other.SupportsCloudSave
                   && RemovedFromEshop == other.RemovedFromEshop
                   && VoucherPossible == other.VoucherPossible
                   && Pictures.Equals(other.Pictures)
                   && Popularity == other.Popularity
                   && (PriceHistory ?? Enumerable.Empty<PriceRecord>()).SequenceEqual(other.PriceHistory ?? Enumerable.Empty<PriceRecord>())
                   && (Nsuids ?? Enumerable.Empty<string>()).SequenceEqual(other.Nsuids ?? Enumerable.Empty<string>())
                   && (Categories ?? Enumerable.Empty<string>()).SequenceEqual(other.Categories ?? Enumerable.Empty<string>())
                   && (Languages ?? Enumerable.Empty<string>()).SequenceEqual(other.Languages ?? Enumerable.Empty<string>())
                   && (PlayModes ?? Enumerable.Empty<string>()).SequenceEqual(other.PlayModes ?? Enumerable.Empty<string>());
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            hashCode.Add(RegularPrice);
            hashCode.Add(AllTimeLow);
            hashCode.Add(AllTimeHigh);
            hashCode.Add(PriceHistory);
            hashCode.Add(Nsuids);
            hashCode.Add(Title);
            hashCode.Add(Slug);
            hashCode.Add(Description);
            hashCode.Add(Publisher);
            hashCode.Add(Developer);
            hashCode.Add(ReleaseDate);
            hashCode.Add(Categories);
            hashCode.Add(AgeRating);
            hashCode.Add(DownloadSize);
            hashCode.Add(MinPlayers);
            hashCode.Add(MaxPlayers);
            hashCode.Add(Coop);
            hashCode.Add(DemoAvailable);
            hashCode.Add(Languages);
            hashCode.Add(PlayModes);
            hashCode.Add(SupportsCloudSave);
            hashCode.Add(RemovedFromEshop);
            hashCode.Add(VoucherPossible);
            hashCode.Add(Pictures);
            hashCode.Add(Popularity);

            return hashCode.ToHashCode();
        }

        #endregion
    }
}