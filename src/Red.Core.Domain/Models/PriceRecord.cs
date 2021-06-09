using System;
using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Date:d,nq} {Price, nq} {Currency, nq}", Type = "PriceRecord")]
    public sealed class PriceRecord
    {
        public DateTime Date { get; init; }
        public decimal Price { get; init; }

        public string Currency { get; init; } = "";
        public string Country { get; init; } = "";
    }
}