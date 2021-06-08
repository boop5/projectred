using System.Diagnostics;

namespace Red.Core.Domain.Models
{
    public enum EshopSalesStatus
    {
        OnSale,
        NotFound,
        SalesTermination,
        Unreleased,
        Unknown
    }

    public enum EshopGameSorting
    {
        Title,
        ReleaseDate

        // todo: add more fields..
    }

    public enum SortingDirection
    {
        Ascending,
        Descending
    }

    public enum NintendoSystem
    {
        NintendoSwitch,
        NintendoWii,
        NintendoWiiU,
        NintendoDs,
        Nintendo2DsXl,
        Nintendo3Ds,
        Nintendo3DsXl
    }
}