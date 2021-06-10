using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGamePriceDetails
    {
        // todo: add country/region flags for floats. shit cant work like this
        // public float? AllTimeHigh { get; init; }
        // public float? AllTimeLow { get; init; }
        // public float? RegularPrice { get; init; }
        public IReadOnlyCollection<PriceRecord> History { get; init; } = new List<PriceRecord>(0);

        public bool Equals(SwitchGamePriceDetails? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return History.SequenceEqual(other.History);
        }

        public override int GetHashCode()
        {
            return History.GetHashCode();
        }
    }
}