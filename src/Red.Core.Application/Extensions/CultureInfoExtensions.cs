using System.Globalization;

namespace Red.Core.Application.Extensions
{
    public static class CultureInfoExtensions
    {
        public static string GetTwoLetterISORegionName(this CultureInfo cultureInfo)
        {
            var region = new RegionInfo(cultureInfo.LCID);

            return region.TwoLetterISORegionName;
        }
    }
}
