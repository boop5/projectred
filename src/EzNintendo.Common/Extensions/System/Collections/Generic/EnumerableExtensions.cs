using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace EzNintendo.Common.Extensions.System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static TType Random<TType>(this IEnumerable<TType> enumerable)
        {
            return enumerable.Random(_ => true);
        }

        public static TType Random<TType>(this IEnumerable<TType> enumerable, Func<TType, bool> where)
        {
            var array = enumerable.Where(where).ToArray();
            var rnd = new Random();
            var n = rnd.Next(0, array.Length);

            return array[n];
        }

        public static NameValueCollection ToNameValueCollection<T>(this IEnumerable<T> enumerable, Func<T, string> name, Func<T, string> value)
        {
            var collection = new NameValueCollection();

            foreach (var item in enumerable)
            {
                collection.Add(name(item), value(item));
            }

            return collection;
        }

        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
                   .Select((x, i) => new
                   {
                       Index = i,
                       Value = x
                   })
                   .GroupBy(x => x.Index / chunkSize)
                   .Select(x => x.Select(v => v.Value).ToList());
        }
    }
}