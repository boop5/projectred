using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EzNintendo.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace EzNintendo.Data.Nintendo
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [Table(nameof(GameLanguage), Schema = "Nintendo")]
    public sealed class GameLanguage : ITrackCreated
    {
        public GameLanguage(Game game, string language)
            : this(game.Id, language)
        {
            Game = game;
        }

        public GameLanguage(Guid gameId, string language)
        {
            GameId = gameId;
            Language = language;
        }

        public Guid GameId { get; set; }
        public Game Game { get; set; }
        [MaxLength(20)]
        public string Language { get; set; }
        public DateTime Created { get; set; }

        public static void Setup(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameLanguage>()
                        .HasKey(x => new { x.GameId, x.Language });

            modelBuilder.Entity<GameLanguage>()
                        .HasOne(gl => gl.Game)
                        .WithMany(g => g.GameLanguages)
                        .HasForeignKey(gl => gl.GameId);
        }
    }
}