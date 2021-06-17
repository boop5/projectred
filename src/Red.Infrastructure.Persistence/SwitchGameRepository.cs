using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Red.Core.Application.Extensions;
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

        public async Task<SwitchGame?> GetByFsId(string fsId)
        {
            if (!string.IsNullOrWhiteSpace(fsId))
            {
                var byFsId = await Get().Where(x => x.FsId == fsId).ToListAsync();

                if (byFsId.Count > 1)
                {
                    // todo: log warning
                }

                if (byFsId.Count == 1)
                {
                    return byFsId.Single();
                }
            }

            return null;
        }

        public async Task<SwitchGame?> GetByNsuid(string nsuid)
        {
            // todo: check region
            var allEntities = await Get().Select(x => new {x.FsId, x.Nsuids}).ToListAsync();
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

            return await GetByFsId(matches.Single().FsId);
        }

        public async Task<IReadOnlyList<SwitchGame>> GetGamesForMediaQuery(CultureInfo culture)
        {
            var region = culture.GetTwoLetterISORegionName();
            var gs = await Context.Games.Select(
                                      x => new SwitchGame
                                      {
                                          FsId = x.FsId,
                                          Media = x.Media,
                                          EshopUrl = x.EshopUrl
                                      })
                                  .ToListAsync();
            return gs.Where(x => !string.IsNullOrWhiteSpace(x.EshopUrl[region]))
                     //.Where(x => x.Media[region]?.LastUpdated < DateTime.Today)
                     .OrderBy(x => x.FsId)
                     .ToList();
        }

        public async Task<IReadOnlyList<SwitchGame>> GetGamesForPriceQuery()
        {
            var gamesToUpdate = await Context.Games
                                             .OrderByDescending(x => x.ReleaseDate)
                                             .Select(
                                                 x => new SwitchGame
                                                 {
                                                     Nsuids = x.Nsuids,
                                                     FsId = x.FsId,
                                                     Price = x.Price,
                                                     Title = x.Title
                                                 })
                                             .ToListAsync();

            // todo: what are we gonna do about games with multiple nsuids?
            return gamesToUpdate.Where(x => x.Nsuids.Any())
                                // todo: how to handle different games with the same nsuid? excluded for now
                                .DistinctBy(x => x.Nsuids[0])
                                .ToList();
        }

        public async Task<SwitchGame?> UpdateAsync(string fsId, Func<SwitchGame, SwitchGame> updateFunc)
        {
            var entity = await GetByFsId(fsId);

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