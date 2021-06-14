using System;
using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record ContentRating
    {
        public int Age { get; init; }
        public List<string> ContentDescriptors { get; init; } = new();
        public bool Provisional { get; init; }
        public string System { get; init; }

        #region Equality

        public bool Equals(ContentRating? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Age == other.Age 
                   && ContentDescriptors.SequenceEqual(other.ContentDescriptors) 
                   && Provisional == other.Provisional 
                   && System == other.System;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Age, ContentDescriptors, Provisional, System);
        }

        #endregion
    }
}