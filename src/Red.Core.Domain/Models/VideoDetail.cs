using System;

namespace Red.Core.Domain.Models
{
    public sealed record VideoDetail
    {
        /// <summary>
        ///     Gets a value which represents the duration of the video in ms.
        /// </summary>
        public long Duration { get; init; }

        public string? PreviewImage { get; init; }
        public string? Title { get; init; }
        public string Url { get; init; }

        public bool Equals(VideoDetail? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Title == other.Title 
                   && Url == other.Url 
                   && PreviewImage == other.PreviewImage 
                   && Duration == other.Duration;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Url, PreviewImage, Duration);
        }
    }
}