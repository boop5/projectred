using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGamePriceDetails
    {
        public bool OnDiscount { get; init; }
        public List<UndatedPriceRecord> AllTimeLow { get; init; } = new(0);
        public List<UndatedPriceRecord> AllTimeHigh { get; init; } = new(0);
        public List<UndatedPriceRecord> RegularPrice { get; init; } = new(0);
        public List<DatedPriceRecord> History { get; init; } = new(0);

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