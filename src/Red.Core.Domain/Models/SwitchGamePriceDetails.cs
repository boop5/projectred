using System;
using System.Collections.Generic;

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

            return OnDiscount == other.OnDiscount 
                   && RegularPrice.Equals(other.RegularPrice) 
                   && History.Equals(other.History) 
                   && AllTimeLow.Equals(other.AllTimeLow) 
                   && AllTimeHigh.Equals(other.AllTimeHigh) 
                   && SalesStatus == other.SalesStatus;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OnDiscount, RegularPrice, History, AllTimeLow, AllTimeHigh, (int) SalesStatus);
        }
    }
}