using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace EzNintendo.Data.Nintendo
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [Table(nameof(GameCategory), Schema = "Nintendo")]
    public sealed class GameCategory
    {
        public GameCategory(Game game, string category)
            : this(game.Id, category)
        {
            Game = game;
        }

        public GameCategory(Guid gameId, string category)
        {
            GameId = gameId;
            Category = category;
        }

        public Guid GameId { get; set; }
        public Game Game { get; set; }

        public string Category { get; set; }

        public static void Setup(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameCategory>()
                        .HasKey(x => new { x.GameId, x.Category });

            modelBuilder.Entity<GameCategory>()
                        .HasOne(gl => gl.Game)
                        .WithMany(g => g.GameCategories)
                        .HasForeignKey(gl => gl.GameId);
        }
    }
}