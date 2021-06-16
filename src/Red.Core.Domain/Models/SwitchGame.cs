using System;
using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGame
    {
        public List<string> Categories { get; init; } = new(0);
        public List<HexColor> Colors { get; init; } = new(0);
        public CountryDictionary<ContentRating> ContentRating { get; init; } = new();
        public CountryDictionary<string> Description { get; init; } = new();
        public CountryDictionary<string> EshopUrl { get; init; } = new();
        public string? FsId { get; init; }
        public List<string> Languages { get; init; } = new(0);

        public SwitchGameMedia Media { get; init; } = new();
        public List<string> Nsuids { get; init; } = new(0);
        public SwitchGamePlayModes PlayModes { get; init; } = new();

        /// <summary>Meant to use to sort search results.</summary>
        /// <remarks>Higher numbers mean less popularity.</remarks>
        public CountryDictionary<int> Popularity { get; init; } = new();

        public CountryDictionary<SwitchGamePriceDetails> Price { get; init; } = new();
        public string ProductCode { get; init; } = "";
        public string Region { get; init; } = "";

        #region Optional

        public string? Title { get; init; }
        public string? Slug { get; init; }
        public string? Publisher { get; init; }
        public string? Developer { get; init; }
        public DateTime? ReleaseDate { get; init; }
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

            return ProductCode == other.ProductCode
                   && FsId == other.FsId
                   && Region == other.Region
                   && Categories.SequenceEqual(other.Categories) 
                   && Languages.SequenceEqual(other.Languages) 
                   && Nsuids.SequenceEqual(other.Nsuids)
                   && Colors.SequenceEqual(other.Colors) 
                   && Media.Equals(other.Media) 
                   && PlayModes.Equals(other.PlayModes)
                   && Price.Equals(other.Price) 
                   && EshopUrl.Equals(other.EshopUrl) 
                   && ContentRating.Equals(other.ContentRating) 
                   && Description.Equals(other.Description)
                   && Popularity == other.Popularity 
                   && Title == other.Title 
                   && Slug == other.Slug
                   && Publisher == other.Publisher 
                   && Developer == other.Developer 
                   && Nullable.Equals(ReleaseDate, other.ReleaseDate)
                   && DownloadSize == other.DownloadSize 
                   && MinPlayers == other.MinPlayers 
                   && MaxPlayers == other.MaxPlayers
                   && Coop == other.Coop 
                   && DemoAvailable == other.DemoAvailable 
                   && SupportsCloudSave == other.SupportsCloudSave
                   && RemovedFromEshop == other.RemovedFromEshop 
                   && VoucherPossible == other.VoucherPossible;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(ProductCode);
            hashCode.Add(FsId);
            hashCode.Add(Region);
            hashCode.Add(Categories);
            hashCode.Add(Languages);
            hashCode.Add(Nsuids);
            hashCode.Add(Colors);
            hashCode.Add(Media);
            hashCode.Add(PlayModes);
            hashCode.Add(Price);
            hashCode.Add(ContentRating);
            hashCode.Add(Description);
            hashCode.Add(Popularity);
            hashCode.Add(Title);
            hashCode.Add(EshopUrl);
            hashCode.Add(Slug);
            hashCode.Add(Publisher);
            hashCode.Add(Developer);
            hashCode.Add(ReleaseDate);
            hashCode.Add(DownloadSize);
            hashCode.Add(MinPlayers);
            hashCode.Add(MaxPlayers);
            hashCode.Add(Coop);
            hashCode.Add(DemoAvailable);
            hashCode.Add(SupportsCloudSave);
            hashCode.Add(RemovedFromEshop);
            hashCode.Add(VoucherPossible);
            return hashCode.ToHashCode();
        }

        #endregion
    }
}