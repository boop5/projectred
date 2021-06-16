using System;
using System.Collections.Generic;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGamePriceDetails
    {
        public Price AllTimeHigh { get; init; } = new();
        public Price AllTimeLow { get; init; } = new();
        public List<DatedPrice> History { get; init; } = new();
        public bool OnDiscount { get; init; }
        public Price RegularPrice { get; init; } = new();
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

            return AllTimeHigh.Equals(other.AllTimeHigh) 
                   && AllTimeLow.Equals(other.AllTimeLow) 
                   && History.Equals(other.History)
                   && OnDiscount == other.OnDiscount 
                   && RegularPrice.Equals(other.RegularPrice) 
                   && SalesStatus == other.SalesStatus;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AllTimeHigh, 
                                    AllTimeLow, 
                                    History,
                                    OnDiscount, 
                                    RegularPrice, 
                                    (int) SalesStatus);
        }
    }
}