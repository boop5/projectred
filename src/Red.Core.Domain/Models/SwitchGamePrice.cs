using System;
using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay(
        "{CurrentPrice,nq} {Currency,nq}" + 
        "{Discounted == true ? \" [DISCOUNTED] Regular: \" + RegularPrice + \" \" + Currency : System.String.Empty,nq}",
        Type = "Switch Game Price"
    )]
    public sealed class SwitchGamePrice
    {
        public float? CurrentPrice { get; init; }
        public bool Discounted { get; init; }
        public string Nsuid { get; }
        public float? RegularPrice { get; init; }
        public string? Currency { get; init; }
        public EshopSalesStatus SalesStatus { get; init; }

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