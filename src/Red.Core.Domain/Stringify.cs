using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain
{ 
    // todo: this should not be in the domain namespace. I'm not sure where to put it though because I want to access it from the domain library (means: i cant put it in the app-layer..)
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
