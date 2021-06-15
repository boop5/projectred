using System.Diagnostics;
using System.Globalization;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Nsuid,nq}", Type = "Eshop Price Query")]
    public sealed class EshopPriceQuery
    {
        public CultureInfo Culture { get; }
        public string Nsuid { get; }

        public EshopPriceQuery(CultureInfo culture, string nsuid)
        {
            Nsuid = nsuid;
            Culture = culture;
        }
    }
}