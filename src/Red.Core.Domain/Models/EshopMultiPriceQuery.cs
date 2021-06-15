using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("Query {Nsuids.Count,nq} Nsuids", Type = "Eshop Price Query")]
    public sealed class EshopMultiPriceQuery
    {
        public CultureInfo Culture { get; }
        public IReadOnlyCollection<string> Nsuids { get; }

        public EshopMultiPriceQuery(CultureInfo culture, IEnumerable<string> nsuids)
        {
            Culture = culture;
            Nsuids = nsuids.ToList();
        }

        public EshopMultiPriceQuery(CultureInfo culture, params string[] nsuids)
        {
            Culture = culture;
            Nsuids = nsuids.ToList();
        }
    }
}