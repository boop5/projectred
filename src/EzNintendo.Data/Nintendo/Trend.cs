using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using EzNintendo.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace EzNintendo.Data.Nintendo
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [DebuggerDisplay("{Country,nq}|{Price}", Name = "{Game.Title,nq}")]
    [Table(nameof(Trend), Schema = "Nintendo")]
    public sealed class Trend : IIdentifiableRecord, ITrackCreated
    {
        public Trend()
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public DateTime Created { get; set; }

        public Guid GameId { get; set; }
        public Game Game { get; set; }

        public float Price { get; set; }
        public string Country { get; set; }

        public static void Setup(ModelBuilder modelBuilder)
        {

            var entity = modelBuilder.Entity<Trend>();

            entity.HasIndex(x => new { x.GameId, x.Country, x.Created });
            entity.HasIndex(p => p.GameId);
            entity.HasIndex(p => p.Country);
            entity.HasIndex(p => p.Created);
        }
    }
}