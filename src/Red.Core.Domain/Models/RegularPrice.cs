using System;
using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed class RegularPrice
    {
        // private readonly List<UndatedPriceRecord> _records = new();
        // public IReadOnlyCollection<UndatedPriceRecord> Records => _records.AsReadOnly();
        public List<UndatedPriceRecord> _records { get; set; } = new();

        public UndatedPriceRecord? this[string country]
        {
            get => _records.SingleOrDefault(x => string.Equals(country, x.Country, StringComparison.InvariantCultureIgnoreCase));
            set
            {
                if (value != null)
                {
                    _records.Add(value);
                }
            }
        }
    }
}