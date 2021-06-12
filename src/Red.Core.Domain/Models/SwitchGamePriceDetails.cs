using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGamePriceDetails
    {
        public bool OnDiscount { get; init; }
        public RegularPrice RegularPrice { get; init; } = new();
        public List<DatedPriceRecord> History { get; init; } = new(0);
        public List<UndatedPriceRecord> AllTimeLow { get; init; } = new(0);
        public List<UndatedPriceRecord> AllTimeHigh { get; init; } = new(0);
        public EshopSalesStatus SalesStatus { get; init; }

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