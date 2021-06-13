using System;
using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record CountryDictionary<T>
    {
        private sealed class Country
        {
            public string Name { get; init; } = "";

            public static Country New(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException();
                }

                return new Country {Name = name};
            }

            private bool Equals(Country other)
            {
                return Name == other.Name;
            }

            public override bool Equals(object? obj)
            {
                return ReferenceEquals(this, obj) || obj is Country other && Equals(other);
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }
        }

        private readonly IDictionary<Country, T> _dictionary = new Dictionary<Country, T>();

        public IReadOnlyCollection<string> Keys => _dictionary.Keys.Select(x => x.Name).ToList();

        public IReadOnlyDictionary<string, T> ToDictionary() => _dictionary.ToDictionary(x => x.Key.Name, x => x.Value);

        public T? this[string country]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(country))
                {
                    return default;
                }

                var key = _dictionary.Keys.SingleOrDefault(x => string.Equals(country, x.Name, StringComparison.InvariantCultureIgnoreCase));

                if (key == null)
                {
                    return default;
                }

                return _dictionary[key];
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(country) && value != null)
                {
                    var key = Country.New(country);
                    _dictionary[key] = value;
                }
            }
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

            return _dictionary.Equals(other._dictionary);
        }

        public override int GetHashCode()
        {
            return _dictionary.GetHashCode();
        }

        #endregion
    }
}