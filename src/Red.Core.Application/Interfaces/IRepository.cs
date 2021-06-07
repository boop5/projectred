using System.Linq;
using System.Threading.Tasks;

namespace Red.Core.Application.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> Get();
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
