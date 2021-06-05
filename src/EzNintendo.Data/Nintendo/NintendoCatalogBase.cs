using System;
using System.Diagnostics.CodeAnalysis;
using EzNintendo.Data.Base;

namespace EzNintendo.Data.Nintendo
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    public abstract class NintendoCatalogBase : IIdentifiableRecord, ITrackCreated
    {
        protected NintendoCatalogBase(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
    }
}