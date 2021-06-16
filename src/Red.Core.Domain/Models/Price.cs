using System;
using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Amount,nq} {Currency,nq}")]
    public sealed record Price
    {
        public float Amount { get; init; }
        public string Currency { get; init; } = "";

        public static Price New(float amount, string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException();
            }

            return new Price() {Amount = amount, Currency = currency};
        }

        #region Equality

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

            return Amount - other.Amount < 0.01
                   && string.Equals(Currency, other.Currency, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }

        #endregion
    }
}