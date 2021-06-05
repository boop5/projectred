using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Data.Base
{
    public abstract class LoggingDbContext : IdentityDbContext
    {
        protected readonly DbContextOptions Options;
        protected readonly ILogger Log;
        protected readonly ILoggerFactory LoggerFactory;

        protected LoggingDbContext(DbContextOptions options, ILogger log, ILoggerFactory loggerFactory) 
            : base(options)
        {
            LoggerFactory = loggerFactory;
            Options = options;
            Log = log;
            LoggerFactory = loggerFactory;
        }

        public override EntityEntry Add(object entity)
        {
            Log.LogTrace("Add {entity}", entity);

            return base.Add(entity);
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            Log.LogTrace("Add {entity}", entity);

            return base.Add(entity);
        }

        public override async ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = new CancellationToken())
        {
            Log.LogTrace("AddAsync {entity}", entity);

            return await base.AddAsync(entity, cancellationToken);
        }

        public override async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        {
            Log.LogTrace("AddAsync {entity}", entity);

            return await base.AddAsync(entity, cancellationToken);
        }

        public override void AddRange(IEnumerable<object> entities)
        {
            Log.LogTrace("AddRange {entities}", entities);

            base.AddRange(entities);
        }

        public override void AddRange(params object[] entities)
        {
            Log.LogTrace("AddRange {entities}", entities);

            base.AddRange(entities);
        }

        public override async Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = new CancellationToken())
        {
            Log.LogTrace("AddRangeAsync {entities}", entities);

            await base.AddRangeAsync(entities, cancellationToken);
        }

        public override async Task AddRangeAsync(params object[] entities)
        {
            Log.LogTrace("AddRangeAsync {entities}", entities);

            await base.AddRangeAsync(entities);
        }

        public override EntityEntry Attach(object entity)
        {
            Log.LogTrace("Attach {entity}", entity);

            return base.Attach(entity);
        }

        public override EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
        {
            Log.LogTrace("Attach {entity}", entity);

            return base.Attach(entity);
        }

        public override void AttachRange(IEnumerable<object> entities)
        {
            Log.LogTrace("AttachRange {entities}", entities);

            base.AttachRange(entities);
        }

        public override void AttachRange(params object[] entities)
        {
            Log.LogTrace("AttachRange {entities}", entities);

            base.AttachRange(entities);
        }

        public override void Dispose()
        {
            Log.LogTrace("Dispose");

            base.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            Log.LogTrace("DisposeAsync");

            await base.DisposeAsync();
        }

        public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        {
            Log.LogTrace("Entry {entity}", entity);

            return base.Entry(entity);
        }

        public override EntityEntry Entry(object entity)
        {
            Log.LogTrace("Entry {entity}", entity);

            return base.Entry(entity);
        }

        public override bool Equals(object obj)
        {
            var result = base.Equals(obj);
            Log.LogTrace("Equals {obj} {result}", obj, result);

            return result;
        }

        public override TEntity Find<TEntity>(params object[] keyValues)
        {
            Log.LogTrace("Find {keyValues}", keyValues);

            return ((DbContext) this).Find<TEntity>(keyValues);
        }

        public override object Find(Type entityType, params object[] keyValues)
        {
            Log.LogTrace("Find {entityType} {keyValues}", entityType, keyValues);

            return base.Find(entityType, keyValues);
        }

        public override async ValueTask<object> FindAsync(Type entityType, params object[] keyValues)
        {
            Log.LogTrace("FindAsync {entityType} {keyValues}", entityType, keyValues);

            return await base.FindAsync(entityType, keyValues);
        }

        public override async ValueTask<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken)
        {
            Log.LogTrace("FindAsync {entityType} {keyValues}", entityType, keyValues);

            return await base.FindAsync(entityType, keyValues, cancellationToken);
        }

        public override async ValueTask<TEntity> FindAsync<TEntity>(params object[] keyValues)
        {
            Log.LogTrace("FindAsync {keyValues}", keyValues);

            return await ((DbContext) this).FindAsync<TEntity>(keyValues);
        }

        public override async ValueTask<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken)
        {
            Log.LogTrace("FindAsync {keyValues}", keyValues);

            return await ((DbContext) this).FindAsync<TEntity>(keyValues, cancellationToken);
        }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            Log.LogTrace("GetHashCode {hashCode}", hashCode);

            return hashCode;
        }

        [Obsolete("Use Set() for entity types without keys")]
        public override DbQuery<TQuery> Query<TQuery>()
        {
            Log.LogTrace("Query");

            return ((DbContext) this).Query<TQuery>();
        }

        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            Log.LogTrace("Remove {entity}", entity);

            return base.Remove(entity);
        }

        public override EntityEntry Remove(object entity)
        {
            Log.LogTrace("Remove {entity}", entity);

            return base.Remove(entity);
        }

        public override void RemoveRange(params object[] entities)
        {
            Log.LogTrace("Remove {entities}", entities);

            base.RemoveRange(entities);
        }

        public override void RemoveRange(IEnumerable<object> entities)
        {
            Log.LogTrace("Remove {entities}", entities);

            base.RemoveRange(entities);
        }

        public override int SaveChanges()
        {
            Log.LogTrace("SaveChanges");

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            Log.LogTrace("SaveChanges {acceptAllChangesOnSuccess}", acceptAllChangesOnSuccess);

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            Log.LogTrace("SaveChangesAsync {acceptAllChangesOnSuccess}", acceptAllChangesOnSuccess);

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            Log.LogTrace("SaveChangesAsync");

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override DbSet<TEntity> Set<TEntity>()
        {
            Log.LogTrace("Set");

            return ((DbContext) this).Set<TEntity>();
        }

        public override EntityEntry Update(object entity)
        {
            Log.LogTrace("Update {entity}", entity);

            return base.Update(entity);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            Log.LogTrace("Update {entity}", entity);

            return base.Update(entity);
        }

        public override void UpdateRange(IEnumerable<object> entities)
        {
            Log.LogTrace("UpdateRange {entities}", entities);

            base.UpdateRange(entities);
        }

        public override void UpdateRange(params object[] entities)
        {
            Log.LogTrace("UpdateRange {entities}", entities);

            base.UpdateRange(entities);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Log.LogTrace("OnConfiguring {optionsBuilder}", optionsBuilder);

            optionsBuilder.UseLoggerFactory(LoggerFactory);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Log.LogTrace("OnModelCreating {builder}", builder);

            base.OnModelCreating(builder);
        }
    }
}