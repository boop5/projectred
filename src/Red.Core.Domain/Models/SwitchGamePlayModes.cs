using System;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGamePlayModes
    {
        public bool Handheld { get; init; }
        public bool Tabletop { get; init; }
        public bool Tv { get; init; }

        public bool Equals(SwitchGamePlayModes? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Tv == other.Tv
                   && Handheld == other.Handheld
                   && Tabletop == other.Tabletop;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tv, Handheld, Tabletop);
        }
    }
}