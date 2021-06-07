using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Persistence
{
    /// <summary>
    /// https://github.com/WolfgangOfner/.NetCoreRepositoryAndUnitOfWorkPattern/t
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private LibraryContext Context { get; }

        protected Repository(IDbContextFactory<LibraryContext> contextFactory)
        {
            Context = contextFactory.CreateDbContext();
        }

        public IQueryable<TEntity> Get()
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

        public async Task<TEntity> AddAsync(TEntity entity)
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

        public async Task<TEntity> UpdateAsync(TEntity entity)
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
    }


    internal sealed class SwitchGameRepository : Repository<SwitchGame>, ISwitchGameRepository
    {
        public SwitchGameRepository(IDbContextFactory<LibraryContext> contextFactory) 
            : base(contextFactory)
        {
        }
    }
}