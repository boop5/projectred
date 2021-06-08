using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Nsuid,nq}", Type ="Eshop Price Query")]
    public sealed class EshopPriceQuery
    {
        public string Nsuid { get; }

        public EshopPriceQuery(string nsuid)
        {
            Nsuid = nsuid;
        }
    }
}