using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;
using Red.Infrastructure.Spider.Settings;

namespace Red.Infrastructure.Spider.Worker
{
    internal sealed class LibrarySpider : Spider
    {
        private readonly IEshop _eshop;
        private readonly ISwitchGameMerger _gameMerger;
        private readonly ISwitchGameRepositoryFactory _repoFactory;
        private readonly WorkerSettings _configuration;

        public LibrarySpider(IAppLogger<LibrarySpider> log,
                             WorkerSettings configuration,
                             ISwitchGameMerger gameMerger,
                             ISwitchGameRepositoryFactory repoFactory,
                             IEshop eshop)
            : base(log, configuration.LibrarySpider)
        {
            _configuration = configuration;
            _gameMerger = gameMerger;
            _repoFactory = repoFactory;
            _eshop = eshop;
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

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            foreach (var culture in _configuration.Cultures)
            {
                var queries = await BuildQueries(culture, _configuration.LibrarySpider.QuerySize);
                var tasks = queries.Select(ProcessQuery);

                await Task.WhenAll(tasks);
            }
        }

        private async Task ProcessQuery(EshopGameQuery query)
        {
            try
            {
                var repo = _repoFactory.Create();
                var games = await _eshop.SearchGames(query);
                Log.LogDebug("Process {count} games", games.Count);

                foreach (var game in games)
                {
                    await UpdateGame(query.Culture, repo, game);
                }
            }
            catch (Exception e)
            {
                Log.LogWarning(e, "Failed to process query {query}", query);
            }
        }

        private async Task UpdateGame(CultureInfo culture, ISwitchGameRepository repo, SwitchGame game)
        {
            var lang = culture.TwoLetterISOLanguageName;

            try
            {
                var dbEntity = await repo.GetMatchingGame(game, culture);

                if (dbEntity == null)
                {
                    Log.LogInformation("Add new game {title} ({fsId})", game.Title[lang] ?? "", game.FsId);

                    // todo: handle slug issue (minefield ...)
                    await repo.AddAsync(game);
                }
                else
                {
                    var updatedEntity = _gameMerger.MergeLibrary(dbEntity, game);

                    if (!Equals(updatedEntity, dbEntity))
                    {
                        Log.LogInformation(
                            "Update existing Game \"{title}\" [{fsId}]",
                            updatedEntity.Title[lang] ?? "",
                            updatedEntity.FsId);
                        await repo.UpdateAsync(updatedEntity);
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogWarning(e, "Failed to update game {game} ({fsId})", game.Title[lang] ?? "", game.FsId);
            }
        }
    }
}