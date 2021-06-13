using System;

namespace Red.Core.Domain.Models
{
    public sealed class EshopSalesQuery
    {
        public string Country { get; private init; } = null!;
        public int Offset { get; private init; }
        public string Locale { get; private init; } = null!;
        public int Count { get; private init; }

        private EshopSalesQuery()
        {
        }

        public static EshopSalesQuery New(string country, string locale, int start, int count)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentException();
            }

            if (string.IsNullOrWhiteSpace(locale))
            {
                throw new ArgumentException();
            }

            if (count < 0 || count > 30)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (start != 0 && start % count != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "count needs to be a multiple of start");
            }

            return new EshopSalesQuery
            {
                Country = country,
                Locale = locale,
                Offset = start,
                Count = count
            };
        }
    }
}