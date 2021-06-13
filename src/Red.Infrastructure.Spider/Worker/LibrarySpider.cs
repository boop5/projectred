using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider.Worker
{
    public class LibrarySpider : TimedWorker
    {
        private readonly IEshop _eshop;
        private readonly IServiceProvider _sp;

        public LibrarySpider(IAppLogger<LibrarySpider> log,
                             IServiceProvider sp,
                             IEshop eshop)
            : base(log)
        {
            _sp = sp;
            _eshop = eshop;
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }

        protected override TimeSpan GetInitialDelay()
        {
            return TimeSpan.FromMinutes(110);
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            var start = 0;
            var increment = 200;
            var end = await _eshop.GetTotalGames();
            var taskCount = (int) Math.Ceiling(1f * end / increment);
            var tasks = new List<Task>(taskCount);

            for (var i = start; i < end; i += increment)
            {
                var task = ProcessQuery(new EshopGameQuery {Index = i, Offset = increment});
                tasks.Add(task);
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
                    // todo: add region in request
                    var dbEntity = await repo.GetByProductCode(game.ProductCode);
                    var existsInDb = dbEntity != null;

                    // todo: handle slug issue (minefield ...)
                    if (existsInDb)
                    {
                        if (!Equals(dbEntity, game))
                        {
                            await repo.UpdateAsync(game);
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