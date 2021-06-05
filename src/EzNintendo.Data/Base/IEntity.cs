using System;
using System.ComponentModel.DataAnnotations;

namespace EzNintendo.Data.Base
{
    public interface ITrackCreated
    {
        DateTime Created { get; set; }
    }

    public interface ITrackUpdated
    {
        DateTime Updated { get; set; }
    }

    public interface IIdentifiableRecord
    {
        [Key]
        Guid Id { get; set; }
    }
}