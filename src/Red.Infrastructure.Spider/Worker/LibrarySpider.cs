using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider.Worker
{
    internal sealed class LibrarySpider : Spider
    {
        private readonly IEshop _eshop;
        private readonly ISwitchGameMerger _gameMerger;
        private readonly IServiceProvider _sp;

        public LibrarySpider(IAppLogger<LibrarySpider> log,
                             LibrarySpiderConfiguration configuration,
                             ISwitchGameMerger gameMerger,
                             IServiceProvider sp,
                             IEshop eshop)
            : base(log, configuration)
        {
            _gameMerger = gameMerger;
            _sp = sp;
            _eshop = eshop;
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            // todo: use proper country/locale
            var culture = new CultureInfo("en-DE");
            var start = 0;
            var increment = 200;
            var end = await _eshop.GetTotalGames(culture);
            var taskCount = (int) Math.Ceiling(1f * end / increment);
            var tasks = new List<Task>(taskCount);

            for (var i = start; i < end; i += increment)
            {
                var task = ProcessQuery(new EshopGameQuery(culture) {Index = i, Offset = increment});
                tasks.Add(task);
                break;
            }

            await Task.WhenAll(tasks);
        }

        private async Task ProcessQuery(EshopGameQuery query)
        {
            try
            {
                var games = await _eshop.SearchGames(query);
                Log.LogInformation("Process {count} games", games.Count);

                await using var repo = (ISwitchGameRepository) _sp.GetRequiredService(typeof(ISwitchGameRepository));

                foreach (var game in games)
                {
                    var dbEntity = await repo.GetMatchingGame(game, query.Culture);

                    // todo: handle slug issue (minefield ...)
                    if (dbEntity != null)
                    {
                        var updatedEntity = _gameMerger.Merge(dbEntity, game);

                        if (!Equals(updatedEntity, dbEntity))
                        {
                            Log.LogInformation("Update existing Game \"{title}\" [{productCode}]", updatedEntity.Title ?? "", updatedEntity.ProductCode);
                            await repo.UpdateAsync(updatedEntity);
                        }
                    }
                    else
                    {
                        await repo.AddAsync(game);
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