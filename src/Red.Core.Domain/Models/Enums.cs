namespace Red.Core.Domain.Models
{
    public enum EshopSalesStatus
    {
        OnSale,
        NotFound,
        SalesTermination,
        Unreleased,
        PreOrder,
        Unknown = 0 // = 0 marks as default value
    }

    public enum EshopGameSorting
    {
        Title,
        ReleaseDate = 0

        // todo: add more fields..
    }

    public enum SortingDirection
    {
        Ascending = 0,
        Descending
    }

    public enum NintendoSystem
    {
        NintendoSwitch = 0,
        NintendoWii,
        NintendoWiiU,
        NintendoDs,
        Nintendo2DsXl,
        Nintendo3Ds,
        Nintendo3DsXl
    }
}