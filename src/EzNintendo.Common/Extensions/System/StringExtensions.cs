namespace EzNintendo.Common.Extensions.System
{
    public static class StringExtensions
    {
        public static string Truncate(this string txt, int threshold, string suffix = "...")
        {
            if (txt.Length > threshold)
            {
                var sub = txt.Substring(0, threshold - suffix.Length);

                return $"{sub} {suffix}";
            }

            return txt;
        }
    }
}