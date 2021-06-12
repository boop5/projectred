using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed class RegularPrice
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public List<UndatedPriceRecord> _records { get; init; } = new();

        private UndatedPriceRecord? GetByCountry(string country)
        {
            return _records.SingleOrDefault(x => string.Equals(country, x.Country, StringComparison.InvariantCultureIgnoreCase));
        }

        public UndatedPriceRecord? this[string country]
        {
            get => GetByCountry(country);
            set
            {
                if (value != null)
                {
                    var existingRecord = GetByCountry(country);

                    if (existingRecord != null)
                    {
                        _records.Remove(existingRecord);
                    }
                    
                    _records.Add(value);
                }
            }
        }

        private bool Equals(RegularPrice other)
        {
            return _records.SequenceEqual(other._records);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is RegularPrice other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _records.GetHashCode();
        }
    }
}