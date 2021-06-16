using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Red.Core.Application.Extensions;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;
using Red.Infrastructure.Spider.Settings;

namespace Red.Infrastructure.Spider.Worker
{
    internal sealed class PriceSpider : Spider
    {
        private readonly WorkerSettings _configuration;
        private readonly IEshop _eshop;
        private readonly ISwitchGameMerger _gameMerger;
        private readonly ISwitchGameRepositoryFactory _repoFactory;

        public PriceSpider(IAppLogger<PriceSpider> log,
                           WorkerSettings configuration,
                           IEshop eshop,
                           ISwitchGameMerger gameMerger,
                           ISwitchGameRepositoryFactory repoFactory)
            : base(log, configuration.PriceSpider)
        {
            _configuration = configuration;
            _eshop = eshop;
            _gameMerger = gameMerger;
            _repoFactory = repoFactory;
        }

        private async Task<List<SwitchGame>> GetGamesToUpdate()
        {
            var repo = _repoFactory.Create();
            var gamesToUpdate = await repo.Get()
                                          .OrderByDescending(x => x.ReleaseDate)
                                          .Select(
                                              x => new SwitchGame
                                              {
                                                  Nsuids = x.Nsuids,
                                                  FsId = x.FsId,
                                                  ProductCode = x.ProductCode,
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

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            foreach (var culture in _configuration.Cultures)
            {
                Log.LogDebug("Process {culture}", culture);
                var gamesToUpdate = await GetGamesToUpdate();
                var chunks = gamesToUpdate.ChunkBy(50).ToList();

                foreach (var groupOfChunks in chunks.ChunkBy(_configuration.PriceSpider.ChunkCount))
                {
                    var tasks = groupOfChunks.Select(x => UpdateGames(culture, x.ToList()));

                    await Task.WhenAll(tasks);
                }
            }
        }

        private async Task ProcessQuery(CultureInfo culture, IReadOnlyCollection<SwitchGame> games, EshopMultiPriceQuery query)
        {
            try
            {
                var prices = await _eshop.GetPrices(query);

                foreach (var price in prices)
                {
                    await UpdateGame(culture, games, price);
                }
            }
            catch (Exception e)
            {
                Log.LogWarning(e, "Failed to process query {query}", query);
            }
        }

        private async Task UpdateGame(CultureInfo culture, IReadOnlyCollection<SwitchGame> games, SwitchGamePrice price)
        {
            try
            {
                var repo = _repoFactory.Create();
                var game = games.First(x => x.Nsuids.Contains(price.Nsuid));
                var entity = (await repo.GetByFsId(game.FsId!))!;
                var merged = _gameMerger.MergePrice(culture, entity, price);

                if (!entity.Price.Equals(merged.Price))
                {
                    Log.LogInformation("Update price for {title} ({FsId})", game.Title ?? "", game.FsId ?? "");
                    await repo.UpdateAsync(merged);
                }
            }
            catch (Exception e)
            {
                Log.LogWarning(e, "Failed to update price");
            }
        }

        private async Task UpdateGames(CultureInfo culture, IReadOnlyCollection<SwitchGame> games)
        {
            var nsuids = games.Select(x => x.Nsuids[0]);
            var query = new EshopMultiPriceQuery(culture, nsuids);
            await ProcessQuery(culture, games, query);
        }
    }
}