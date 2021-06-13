using System;
using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGameMedia
    {
        public DateTime? LastUpdated { get; init; }

        /// <summary>uri to image.</summary>
        public ImageDetail? Cover { get; init; }
        
        public ImageDetail? HeroBanner { get; init; }

        /// <summary>List of uris.</summary>
        public List<ImageDetail> Screenshots { get; init; } = new List<ImageDetail>();
        public List<VideoDetail> Videos { get; init; } = new List<VideoDetail>();

        public bool Equals(SwitchGameMedia? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Screenshots.SequenceEqual(other.Screenshots)
                   && Videos.SequenceEqual(other.Videos)
                   && Cover == other.Cover
                   && Nullable.Equals(LastUpdated, other.LastUpdated);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Screenshots, Videos, Cover, LastUpdated);
        }
    }
}