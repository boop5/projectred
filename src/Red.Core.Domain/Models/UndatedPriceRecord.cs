using System;
using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Price.Amount, nq} {Price.Currency, nq}", Type = "UndatedPriceRecord")]
    public sealed record UndatedPriceRecord
    {
        public string Country { get; init; } = "";
        public Price Price { get; init; } = new();

        public bool Equals(UndatedPriceRecord? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Country == other.Country && Price.Equals(other.Price);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Country, Price);
        }

        public static UndatedPriceRecord New(string country, float amount, string currency)
        {
            return new()
            {
                Country = country,
                Price = new Price
                {
                    Amount = amount,
                    Currency = currency
                }
            };
        }
    }


    [DebuggerDisplay("{Date:d,nq} {Price, nq}", Type = "DatedPriceRecord")]
    public sealed record DatedPriceRecord
    {
        public string Country { get; init; } = "";
        public DateTime Date { get; init; }
        public Price Price { get; init; } = new();

        public static DatedPriceRecord New(string country, float amount, string currency)
        {
            return new()
            {
                Country = country,
                Date = DateTime.UtcNow,
                Price = new Price
                {
                    Amount = amount,
                    Currency = currency
                }
            };
        }
    }
}