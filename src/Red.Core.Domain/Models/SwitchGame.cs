﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGame
    {
        public string ProductCode { get; init; } = "";
        public string Region { get; init; } = "";

        public List<string> Categories { get; init; } = new List<string>();
        public List<string> Languages { get; init; } = new List<string>();
        public List<string> Nsuids { get; init; } = new List<string>();

        public SwitchGamePictures Pictures { get; init; } = new();
        public SwitchGamePlayModes PlayModes { get; init; } = new();
        public SwitchGamePriceDetails Price { get; init; } = new();

        /// <summary>Meant to use to sort search results.</summary>
        /// <remarks>Higher numbers mean less popularity.</remarks>
        public int Popularity { get; init; } = int.MaxValue;

        #region Optional

        public string? Title { get; init; }
        public string? EshopUrl { get; init; }
        public string? Slug { get; init; }
        public string? Description { get; init; }
        public string? Publisher { get; init; }
        public string? Developer { get; init; }
        public DateTime? ReleaseDate { get; init; }
        public int? AgeRating { get; init; }
        public int? DownloadSize { get; init; }
        public int? MinPlayers { get; init; }
        public int? MaxPlayers { get; init; }
        public bool? Coop { get; init; }
        public bool? DemoAvailable { get; init; }
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

            return Title == other.Title
                   && Slug == other.Slug
                   && EshopUrl == other.EshopUrl
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
                   && Popularity == other.Popularity
                   && Nullable.Equals(ReleaseDate, other.ReleaseDate)
                   && Pictures.Equals(other.Pictures)
                   && Price.Equals(other.Price)
                   && Categories.SequenceEqual(other.Categories)
                   && Languages.SequenceEqual(other.Languages)
                   && Nsuids.SequenceEqual(other.Nsuids)
                   && PlayModes.Equals(other.PlayModes);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            hashCode.Add(Price);
            hashCode.Add(Nsuids);
            hashCode.Add(Title);
            hashCode.Add(Slug);
            hashCode.Add(EshopUrl);
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