using System;
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

        public Task<SwitchGame> GetByProductCode(string productCode)
        {
            return Context.Games
                          .AsNoTracking()
                          .SingleOrDefaultAsync(x => x.ProductCode == productCode);
        }

        public async Task<SwitchGame> UpdateAsync(string productCode, Func<SwitchGame, SwitchGame> updateFunc)
        {
            var entity = await GetByProductCode(productCode);
            var updatedEntity = updateFunc(entity);

            return await UpdateAsync(updatedEntity);
        }
    }
}