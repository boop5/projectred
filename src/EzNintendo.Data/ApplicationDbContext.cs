using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EzNintendo.Data.Base;
using EzNintendo.Data.Nintendo;
using EzNintendo.Data.QueryCollections;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace EzNintendo.Data
{
   

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated by DI.")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class ApplicationDbContext : IdentityDbContext
    {
        private readonly NLogLoggerFactory _loggerFactory;
        private readonly ILogger _log;

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter", Justification = "False Positive.")]
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    NLogLoggerFactory loggerFactory,
                                    IGameQueries gameQueries,
                                    ILogger<ApplicationDbContext> log)
            : base(options)
        {
            log.LogDebug($"Create new {nameof(ApplicationDbContext)}.");

            _loggerFactory = loggerFactory;
            _log = log;

            GameQueries = gameQueries;
            GameQueries.SetContext(this);

            ChangeTracker.StateChanged += (s, e) => OnChanged(e.Entry);
            ChangeTracker.Tracked += (s, e) => OnChanged(e.Entry);
        }

        public IGameQueries GameQueries { get; }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Gets set by EfCore.")]
        public DbSet<Game> Games { get; set; }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Gets set by EfCore.")]
        public DbSet<Trend> Trend { get; set; }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Gets set by EfCore.")]
        public DbSet<GameCategory> Categories { get; set; }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Gets set by EfCore.")]
        public DbSet<GameLanguage> Languages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _log.LogTrace("OnConfiguring {optionsBuilder}", optionsBuilder);

            // optionsBuilder.EnableSensitiveDataLogging().UseLoggerFactory(_loggerFactory);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Tables
            Game.Setup(builder);
            Nintendo.Trend.Setup(builder);

            // Joins
            GameLanguage.Setup(builder);
            GameCategory.Setup(builder);
        }

        private void OnChanged(EntityEntry e)
        {
            var now = DateTime.UtcNow;

            switch (e.State)
            {
                case EntityState.Added:
                {
                    if (e.Entity is IIdentifiableRecord e0)
                    {
                        if (e0.Id == default)
                        {
                            e0.Id = Guid.NewGuid();
                        }
                    }

                    if (e.Entity is ITrackCreated e1)
                    {
                        if (e1.Created == default)
                        {
                            _log.LogTrace("Set 'Created' to {now} for added Entity {entity}", now.ToString("u"), e1);
                            e1.Created = now;
                        }
                    }

                    if (e.Entity is ITrackUpdated e2)
                    {
                        if (e2.Updated == default)
                        {
                            _log.LogTrace("Set 'Updated' to {now} for added Entity {entity}", now.ToString("u"), e2);
                            e2.Updated = DateTime.UtcNow;
                        }
                    }

                    break;
                }

                case EntityState.Modified:
                {
                    if (e.Entity is ITrackUpdated entity)
                    {
                        _log.LogTrace("Set 'Updated' to {now} for modified Entity {entity}", now.ToString("u"), entity);
                        entity.Updated = DateTime.UtcNow;
                    }

                    break;
                }

                case EntityState.Detached: break;
                case EntityState.Unchanged: break;
                case EntityState.Deleted: break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}