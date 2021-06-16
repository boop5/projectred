using System;
using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGameMedia
    {
        public static SwitchGameMedia Default { get; } = new();
      
        public ImageDetail? Cover { get; init; }
        public ImageDetail? HeroBanner { get; init; }
        public DateTime? LastUpdated { get; init; }
        public List<ImageDetail> Screenshots { get; init; } = new();
        public List<VideoDetail> Videos { get; init; } = new();

        #region Equality

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
                   && Equals(HeroBanner, other.HeroBanner)
                   && Equals(Cover, other.Cover)
                   && Nullable.Equals(LastUpdated, other.LastUpdated);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Screenshots, Videos, Cover, LastUpdated, HeroBanner);
        }

        #endregion
    }
}