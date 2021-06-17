using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Red.Core.Application.Interfaces
{
    public interface IRepository<TEntity> : IDisposable, IAsyncDisposable
        where TEntity : class, new()
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<IReadOnlyCollection<TEntity>> AddAsync(IReadOnlyCollection<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);
        Task<IReadOnlyCollection<TEntity>> UpdateAsync(IReadOnlyCollection<TEntity> entities);
    }
}