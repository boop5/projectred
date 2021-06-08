using System.Collections.Generic;

namespace Red.Core.Domain.Models
{
    public sealed class EshopMultiPriceQuery
    {
        public List<string> Nsuids { get; }

        public EshopMultiPriceQuery(List<string> nsuids)
        {
            Nsuids = nsuids;
        }
    }
}