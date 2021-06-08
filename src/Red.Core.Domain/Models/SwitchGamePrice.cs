using System;

namespace Red.Core.Domain.Models
{
    public sealed class SwitchGamePrice
    {
        public float? CurrentPrice { get; init; }
        public bool Discounted { get; init; } = false;
        public string Nsuid { get; }
        public float? RegularPrice { get; init; }
        public EshopSalesStatus? SalesStatus { get; init; }

        public SwitchGamePrice(string nsuid)
        {
            if (string.IsNullOrWhiteSpace(nsuid))
            {
                throw new ArgumentException("Nsuid cant be null or empty", nameof(nsuid));
            }

            Nsuid = nsuid;
        }
    }
}