using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Red.Core.Application.Common;
using Red.Core.Application.Extensions;
using Red.Core.Application.Features.GameFeatures.Commands;
using Red.Core.Application.Features.GameFeatures.Queries;
using Red.Core.Application.Interfaces;
using Red.Infrastructure.Spider.Settings;

namespace Red.Infrastructure.Spider.Worker
{
    internal sealed class LibrarySpider : Spider
    {
        private readonly ICommandBus _commandBus;
        private readonly WorkerSettings _configuration;
        private readonly IQueryBus _queryBus;

        public LibrarySpider(IAppLogger<LibrarySpider> log,
                             WorkerSettings configuration,
                             ICommandBus commandBus,
                             IQueryBus queryBus)
            : base(log, configuration.LibrarySpider)
        {
            _configuration = configuration;
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            foreach (var culture in _configuration.Cultures)
            {
                var games = await _queryBus.Send(new GetGamesFromEshopQuery(culture), stoppingToken);

                foreach (var chunk in games.ChunkBy(200))
                {
                    var tasks = new List<Task>(200);

                    foreach (var game in chunk)
                    {
                        tasks.Add(_commandBus.Send(new UpdateGameCommand(culture, game)));
                    }

                    await Task.WhenAll(tasks);
                }
            }
        }
    }
}