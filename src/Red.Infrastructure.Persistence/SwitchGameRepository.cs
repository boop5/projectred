using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Persistence
{
    internal sealed class SwitchGameRepository : Repository<SwitchGame>, ISwitchGameRepository
    {
        public SwitchGameRepository(IDbContextFactory<LibraryContext> contextFactory)
            : base(contextFactory)
        {
        }

        public async Task<SwitchGame?> GetByProductCode(string productCode)
        {
            return await Get().SingleOrDefaultAsync(x => x.ProductCode == productCode);
        }

        public async Task<SwitchGame?> GetByNsuid(string nsuid)
        {
            var allEntities = await Context.Games.Select(x => new {x.ProductCode, x.Nsuids}).ToListAsync();
            var matches = allEntities.Where(x => x.Nsuids.Contains(nsuid)).ToList();

            if (matches.Count > 1)
            {
                // todo: log warning
                return null;
            }

            if (matches.Count == 0)
            {
                return null;
            }

            return await GetByProductCode(matches.Single().ProductCode);
        }

        public async Task<SwitchGame?> UpdateAsync(string productCode, Func<SwitchGame, SwitchGame> updateFunc)
        {
            var entity = await GetByProductCode(productCode);
            
            if (entity == null)
            {
                // todo: log warning
                return null;
            }

            var updatedEntity = updateFunc(entity);

            return await UpdateAsync(updatedEntity);
        }
    }
}