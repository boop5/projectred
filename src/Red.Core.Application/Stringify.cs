using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Application
{
    public static class Stringify
    {
        public static string Build(object? o)
        {
            if(o == null)
            {
                return "NULL";
            }

            if (o is IEnumerable<object> enumerable)
            {
                var values = string.Join(",", enumerable.Select(Build));
                return $"[{values}]";
            }

            if(o is string s)
            {
                return $"\"{s}\"";
            }

            return o.ToString() ?? "NULL";
        }
    }
}
