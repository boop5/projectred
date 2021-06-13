using System.Collections.Generic;

namespace Red.Core.Domain.Models
{
    public sealed class SwitchGameSale
    {
        public List<HexColor> Colors { get; init; } = new();
        public List<ImageDetail> Screenshots { get; init; } = new();
        public string Title { get; init; } = "";
        public string Nsuid { get; init; } = "";
        public bool IsNew { get; init; }
        public string? HeroBannerUrl { get; init; }
        public int? UskRating { get; init; }
    }
}