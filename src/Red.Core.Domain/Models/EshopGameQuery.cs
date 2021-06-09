using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    [DebuggerDisplay("{Term,nq} [{Index,nq}:{Offset,nq}]", Type = "Eshop Game Query")]
    public sealed class EshopGameQuery
    {
        public int Index { get; init; }
        public int Offset { get; init; } = int.MaxValue;
        public EshopGameSorting SortBy { get; init; } = EshopGameSorting.ReleaseDate;
        public SortingDirection SortingDirection { get; init; } = SortingDirection.Ascending;
        public string Term { get; }

        public EshopGameQuery(string term = "*")
        {
            Term = term;
        }
    }
}