using System;
using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Price.Amount, nq} {Price.Currency, nq}", Type = "UndatedPriceRecord")]
    public sealed record UndatedPriceRecord
    {
        public string Country { get; init; } = "";
        public Price Price { get; init; } = new();

        public static UndatedPriceRecord New(string country, float amount, string currency)
        {
            return new()
            {
                Country = country,
                Price = new Price()
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
        public Price Price { get; init; } = new(); 
        public DateTime Date { get; init; }

        public static DatedPriceRecord New(string country, float amount, string currency)
        {
            return new()
            {
                Country = country,
                Date = DateTime.UtcNow,
                Price = new Price()
                {
                    Amount = amount,
                    Currency = currency
                }
            };
        }
    }
}