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
                return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
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

        private IDictionary<Country, T> Dictionary { get; init; } = new Dictionary<Country, T>();

        public IReadOnlyCollection<string> Keys => Dictionary.Keys.Select(x => x.Name).ToList();

        public IReadOnlyDictionary<string, T> ToDictionary() => Dictionary.ToDictionary(x => x.Key.Name, x => x.Value);

        public T? this[string country]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(country))
                {
                    return default;
                }

                var key = Dictionary.Keys.SingleOrDefault(x => string.Equals(country, x.Name, StringComparison.InvariantCultureIgnoreCase));

                if (key == null)
                {
                    return default;
                }

                return Dictionary[key];
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(country) && value != null)
                {
                    var key = Country.New(country);
                    Dictionary[key] = value;
                }
            }
        }

        public static CountryDictionary<T> New(IReadOnlyDictionary<string, T> dictionary)
        {
            var newDictionary = dictionary
                .ToDictionary(x => Country.New(x.Key), x => x.Value);
            return new() {Dictionary = newDictionary};
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

            if (Dictionary.Count != other.Dictionary.Count)
            {
                return false;
            }

            return !Dictionary.Except(other.Dictionary).Any();
        }

        public override int GetHashCode()
        {
            return Dictionary.GetHashCode();
        }

        #endregion
    }
}