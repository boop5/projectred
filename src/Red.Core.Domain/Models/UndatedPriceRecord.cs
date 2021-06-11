using System;
using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Price.Amount, nq} {Price.Currency, nq}", Type = "UndatedPriceRecord")]
    public sealed record UndatedPriceRecord
    {
        public string Country { get; init; } = "";
        public Price Price { get; init; } = new();
    }


    [DebuggerDisplay("{Date:d,nq} {Price, nq}", Type = "DatedPriceRecord")]
    public sealed record DatedPriceRecord
    {
        public string Country { get; init; } = "";
        public Price Price { get; init; } = new(); 
        public DateTime Date { get; init; }
    }
}