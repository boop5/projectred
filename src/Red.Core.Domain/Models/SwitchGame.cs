using System;
using System.Collections.Generic;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGame
    {
        #region should be in another model 

        public Guid EntityId { get; init; }
        public DateTime EntityCreated { get; init; }
        public DateTime? EntityUpdated { get; init; }

        #endregion

        public float? RegularPrice { get; init; }
        public float? AllTimeLow { get; init; }
        public float? AllTimeHigh { get; init; }
        public List<PriceRecord>? PriceHistory { get; set; }

        public string? Nsuid { get; init; }
        public string? Title { get; init; }
        public string Slug { get; init; } = "";
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

        /// <summary>List of uris.</summary>
        public List<string>? Screenshots { get; init; }
        
        /// <summary>uri to image.</summary>
        public string? Cover { get; init; }

        /// <summary>Meant to use to sort search results.</summary>
        public int Popularity { get; init; } = 0;
    }
}
