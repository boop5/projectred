using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Red.Core.Application.Interfaces;

namespace Red.Core.Application.Features.GameFeatures.Events
{
    public sealed class GameUpdatedEventHandler : IRequestHandler<GameUpdatedEvent>
    {
        private IAppLogger<GameUpdatedEventHandler> Log { get; }

        public GameUpdatedEventHandler(IAppLogger<GameUpdatedEventHandler> log)
        {
            Log = log;
        }

        public Task<Unit> Handle(GameUpdatedEvent request, CancellationToken cancellationToken)
        {
            Log.LogInformation(
                "Update game '{title}' [{fsId}]",
                GameAddedEventHandler.GetTitle(request.Game),
                request.Game.FsId);

            return Unit.Task;
        }
    }
}