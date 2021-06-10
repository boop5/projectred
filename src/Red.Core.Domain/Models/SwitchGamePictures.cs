using System;
using System.Collections.Generic;
using System.Linq;

namespace Red.Core.Domain.Models
{
    public sealed record SwitchGamePictures
    {
        /// <summary>uri to image.</summary>
        public string? Cover { get; init; }

        /// <summary>List of uris.</summary>
        public IReadOnlyCollection<string> Screenshots { get; init; } = new List<string>();

        public bool Equals(SwitchGamePictures? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Screenshots.SequenceEqual(other.Screenshots)
                   && Cover == other.Cover;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Screenshots, Cover);
        }
    }
}