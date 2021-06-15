using System;
using System.Globalization;

namespace Red.Core.Domain.Models
{
    public sealed class EshopSalesQuery
    {
        public int Offset { get; private init; }
        public int Count { get; private init; }
        public CultureInfo Culture { get; private init; } = null!;

        private EshopSalesQuery() {}

        public static EshopSalesQuery New(CultureInfo culture, int start, int count)
        {
            if (culture == null)
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
                Culture = culture,
                Offset = start,
                Count = count
            };
        }
    }
}