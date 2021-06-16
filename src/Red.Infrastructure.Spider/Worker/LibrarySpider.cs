using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider.Worker
{
    internal sealed class LibrarySpider : Spider
    {
        private readonly IEshop _eshop;
        private readonly ISwitchGameMerger _gameMerger;
        private readonly ISwitchGameRepositoryFactory _repoFactory;

        public LibrarySpider(IAppLogger<LibrarySpider> log,
                             LibrarySpiderConfiguration configuration,
                             ISwitchGameMerger gameMerger,
                             ISwitchGameRepositoryFactory repoFactory,
                             IEshop eshop)
            : base(log, configuration)
        {
            _gameMerger = gameMerger;
            _repoFactory = repoFactory;
            _eshop = eshop;
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            // todo: use proper country/locale
            var queries = await BuildQueries(new CultureInfo("en-DE"), 200);
            var tasks = queries.Select(ProcessQuery);

            await Task.WhenAll(tasks);
        }

        private async Task<IReadOnlyCollection<EshopGameQuery>> BuildQueries(CultureInfo culture, int querySize)
        {
            var end = await _eshop.GetTotalGames(culture);
            var queries = new List<EshopGameQuery>();

            for (var start = 0; start < end; start += querySize)
            {
                queries.Add(new EshopGameQuery(culture) {Index = start, Offset = querySize});
            }

            return queries;
        }

        private async Task ProcessQuery(EshopGameQuery query)
        {
            try
            {
                var games = await _eshop.SearchGames(query);
                Log.LogInformation("Process {count} games", games.Count);

                var repo = _repoFactory.Create();

                foreach (var game in games)
                {
                    var dbEntity = await repo.GetMatchingGame(game, query.Culture);

                    if (dbEntity == null)
                    {
                        // todo: handle slug issue (minefield ...)
                        await repo.AddAsync(game);
                    }
                    else
                    {
                        var updatedEntity = _gameMerger.MergeLibrary(dbEntity, game);

                        if (!Equals(updatedEntity, dbEntity))
                        {
                            Log.LogInformation(
                                "Update existing Game \"{title}\" [{productCode}]",
                                updatedEntity.Title ?? "",
                                updatedEntity.ProductCode);
                            await repo.UpdateAsync(updatedEntity);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogWarning(e, "Failed to process query {query}", query);
            }

            Log.LogInformation("Finished processing");
        }
    }
}