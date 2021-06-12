using System;
using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Amount} {Currency}")]
    public sealed record Price
    {
        public float Amount { get; init; }
        public string Currency { get; init; } = "";

        public bool Equals(Price? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return (Amount - other.Amount < 0.01) && Currency == other.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }

        public override string ToString()
        {
            return $"{Amount:F} {Currency}";
        }
    }
}