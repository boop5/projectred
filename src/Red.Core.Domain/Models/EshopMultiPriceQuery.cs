using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("Query {Nsuids.Count,nq} Nsuids", Type ="Eshop Price Query")]
    public sealed class EshopMultiPriceQuery
    {
        private readonly IReadOnlyCollection<string> _nsuids = new List<string>();

        public IReadOnlyCollection<string> Nsuids
        {
            get => _nsuids;
            private init
            {
                if (value.Count < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(Nsuids), "Minimum 1 id required");
                }
                if (value.Count > 50)
                {
                    throw new ArgumentOutOfRangeException(nameof(Nsuids), "Maximum 50 ids allowed");
                }

                _nsuids = value;
            }
        }

        public EshopMultiPriceQuery(List<string> nsuids)
        {
            Nsuids = nsuids;
        }

        public EshopMultiPriceQuery(params string[] nsuids)
        {
            Nsuids = nsuids.ToList();
        }
    }
}