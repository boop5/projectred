using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Red.Core.Domain.Models
{
    public sealed record CountryDictionary<T>
    {
        private IDictionary<string, T> Dictionary { get; init; } = new Dictionary<string, T>();

        public T? this[string key]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    return default;
                }

                var actualKey = Dictionary.Keys.SingleOrDefault(
                    x =>
                        string.Equals(key, x, StringComparison.InvariantCultureIgnoreCase));

                if (actualKey == null)
                {
                    return default;
                }

                return Dictionary[actualKey];
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(key) && value != null)
                {
                    Dictionary[key] = value;
                }
            }
        }

        public IReadOnlyCollection<string> Keys => Dictionary.Keys.ToList();

        public CountryDictionary<T> Merge(CountryDictionary<T> other)
        {
            var newDict = New(ToDictionary());

            foreach (var key in other.Keys)
            {
                if (newDict[key] == null)
                {
                    newDict[key] = other[key];
                }
                else if (!newDict[key]!.Equals(other[key]))
                {
                    newDict[key] = other[key];
                }
            }

            return newDict;
        }

        public static CountryDictionary<T> New(IReadOnlyDictionary<string, T> dictionary)
        {
            return new() {Dictionary = dictionary.ToDictionary(x => x.Key, x => x.Value)};
        }

        public IReadOnlyDictionary<string, T> ToDictionary()
        {
            return Dictionary.ToDictionary(x => x.Key, x => x.Value);
        }

        #region Equality

        public bool Equals(CountryDictionary<T>? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (!Dictionary.Keys.SequenceEqual(other.Dictionary.Keys))
            {
                return false;
            }

            // todo: Equality Comparison is broken here.. ._.

            return !Dictionary.Except(other.Dictionary).Any();
        }

        public override int GetHashCode()
        {
            return Dictionary.GetHashCode();
        }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var key in Keys)
            {
                sb.AppendLine($"{{\"{key}\" = {Stringify.Build(this[key])} }}");
            }

            return sb.ToString();
        }
    }
}