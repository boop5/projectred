using System.Collections.Generic;
using System.Linq;
using System.Text;
using Red.Core.Domain;

namespace Red.Infrastructure.Spider
{
    internal sealed class ObjectDiffBuilder
    {
        private string BuildUpdateString(object? a, object? b)
        {
            var stringA = Stringify.Build(a);
            var stringB = Stringify.Build(b);

            return $"'{stringA}' -> '{stringB}'";
        }

        private bool AreEqual(object? a, object? b)
        {
            if(a == null && b == null)
            {
                return true;
            }

            if (a is IEnumerable<object> enumerableA && b is IEnumerable<object> enumerableB)
            {
                return enumerableA.SequenceEqual(enumerableB);
            }

            return Equals(a, b);
        }

        public string? BuildText(object a, object b)
        {
            if (a.GetType() != b.GetType())
            {
                // todo: LogWarning
                return null;
            }

            if (AreEqual(a, b))
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Merged two entities '{a.GetType().Name}'");

            var hasUpdate = false;
            foreach (var propertyInfo in a.GetType().GetProperties().Where(x => x.CanRead))
            {
                var valueA = propertyInfo.GetValue(a);
                var valueB = propertyInfo.GetValue(b);

                if(!AreEqual(valueA, valueB))
                {
                    hasUpdate = true;
                    sb.AppendLine($"{propertyInfo.Name}: {BuildUpdateString(valueA, valueB)}");
                }
            }

            if (hasUpdate)
            {
                return sb.ToString();
            }

            return null;
        }
    }
}