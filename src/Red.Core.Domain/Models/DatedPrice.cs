using System;
using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Date:d,nq} {Amount, nq} {Currency, nq}", Type = "DatedPriceRecord")]
    public sealed record DatedPrice
    {
        public float Amount { get; init; }
        public string Currency { get; init; } = "";
        public DateTime Date { get; init; } = DateTime.UtcNow;

        public static DatedPrice New(float amount, string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException();
            }

            return new DatedPrice()
            {
                Amount = amount,
                Currency = currency
            };
        }

        #region Equality

        public bool Equals(DatedPrice? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Date.Equals(other.Date) && Amount.Equals(other.Amount) && Currency == other.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Amount, Currency);
        }

        #endregion
    }
}