namespace Red.Core.Domain.Models
{
    public enum EshopSalesStatus
    {
        Unknown = 0, // = 0 marks as default value
        OnSale = 1,
        NotFound = 2,
        SalesTermination = 3,
        Unreleased = 4,
        PreOrder = 5
    }

    public enum EshopGameSorting
    {
        ReleaseDate = 0,
        Title = 1

        // todo: add more fields..
    }

    public enum SortingDirection
    {
        Ascending = 0,
        Descending = 1
    }

    public enum NintendoSystem
    {
        NintendoSwitch = 0,
        NintendoWii = 1,
        NintendoWiiU = 2,
        NintendoDs = 3,
        Nintendo2DsXl = 4,
        Nintendo3Ds = 5,
        Nintendo3DsXl = 6
    }
}