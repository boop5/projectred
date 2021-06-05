using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace EzNintendo.Domain.Nintendo
{
    [DebuggerDisplay("NsuId[{Id}]")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class NsuId
    {
        public NsuId(long id)
        {
            Id = id;
        }

        public long Id { get; }

        public static implicit operator long(NsuId id) => id.Id;
        public static implicit operator long?(NsuId id) => id.Id;

        public static explicit operator NsuId(long id) => new NsuId(id);
        public static explicit operator NsuId(long? id) => id.HasValue ? new NsuId(id.Value) : null;

        private bool Equals(NsuId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is NsuId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}