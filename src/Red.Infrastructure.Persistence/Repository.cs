using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.Persistence
{
    /// <summary>
    /// https://github.com/WolfgangOfner/.NetCoreRepositoryAndUnitOfWorkPattern/t
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected LibraryContext Context { get; }

        protected Repository(IDbContextFactory<LibraryContext> contextFactory)
        {
            Context = contextFactory.CreateDbContext();
        }

        public virtual IQueryable<TEntity> Get()
        {
            try
            {
                return Context.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await Context.AddAsync(entity);
                await Context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public virtual async Task<IReadOnlyCollection<TEntity>> AddAsync(IReadOnlyCollection<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entities must not be null");
            }

            try
            {
                await Context.AddRangeAsync(entities);
                await Context.SaveChangesAsync();

                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entities)} could not be saved: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                Context.Update(entity);
                await Context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public virtual async Task<IReadOnlyCollection<TEntity>> UpdateAsync(IReadOnlyCollection<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAsync)} entities must not be null");
            }

            try
            {
                Context.UpdateRange(entities);
                await Context.SaveChangesAsync();

                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entities)} could not be updated: {ex.Message}");
            }
        }
    }
}