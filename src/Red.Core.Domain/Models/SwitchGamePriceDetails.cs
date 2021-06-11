using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record NintendoPrice
    {
        public string Country { get; init; } = "";
        public float Amount { get; init; }
        public string Currency { get; init; } = "";
    }

    public sealed record SwitchGamePriceDetails
    {
        public bool OnDiscount { get; init; }
        public List<NintendoPrice> AllTimeHigh { get; init; } = new();
        public List<NintendoPrice> AllTimeLow { get; init; } = new();
        public List<NintendoPrice> RegularPrice { get; init; } = new();
        public List<PriceRecord> History { get; init; } = new List<PriceRecord>(0);

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