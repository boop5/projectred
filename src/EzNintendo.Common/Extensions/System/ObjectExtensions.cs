using System.Collections.Generic;

namespace EzNintendo.Common.Extensions.System
{
    /// <remarks>https://stackoverflow.com/a/737159/3450580</remarks>
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> DictionaryFromType(this object obj)
        {
            var dict = new Dictionary<string, object>();

            if (obj == null)
            {
                return dict;
            }

            var type = obj.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj, new object[] { });
                dict.Add(property.Name, value);
            }

            return dict;
        }
    }
}