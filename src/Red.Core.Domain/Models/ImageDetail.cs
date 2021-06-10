using System;

namespace Red.Core.Domain.Models
{
    public sealed record ImageDetail
    {
        public string? Title { get; init; }
        public string Url { get; init; }

        public bool Equals(ImageDetail? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Title == other.Title && Url == other.Url;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Url);
        }
    }
}