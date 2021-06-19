using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Features.GameFeatures.Events
{
    public sealed class GameAddedEventHandler : IRequestHandler<GameAddedEvent>
    {
        private IAppLogger<GameAddedEventHandler> Log { get; }

        public GameAddedEventHandler(IAppLogger<GameAddedEventHandler> log)
        {
            Log = log;
        }

        public Task<Unit> Handle(GameAddedEvent request, CancellationToken cancellationToken)
        {
            Log.LogInformation("Add new game {title} ({fsId})", GetTitle(request.Game), request.Game.FsId);

            return Unit.Task;
        }

        internal static string GetTitle(SwitchGame game)
        {
            if (!string.IsNullOrWhiteSpace(game.Title["en"]))
            {
                return game.Title["en"]!;
            }

            var key = game.Title.Keys.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(game.Title[key]))
            {
                return game.Title[key]!;
            }

            return "NULL";
        }
    }
}