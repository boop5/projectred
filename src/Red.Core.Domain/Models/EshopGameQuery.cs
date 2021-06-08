namespace Red.Core.Domain.Models
{
    public sealed class EshopGameQuery
    {
        public int Index { get; init; } = 0;
        public int Offset { get; init; } = int.MaxValue;
        public EshopGameSorting SortBy { get; init; } = EshopGameSorting.ReleaseDate;
        public SortingDirection SortingDirection { get; init; } = SortingDirection.Ascending;
        public string Term { get; init; } = "*";
    }
}