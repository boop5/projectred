using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Red.Core.Application.Features.GameFeatures.Commands
{
    public class UpdateGameCommandHandler : IRequestHandler<UpdateGameCommand>
    {
        public Task<Unit> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("gnihihihihi xddd update game " + request.Game.Title["de"]);

            return Unit.Task;
        }
    }
}
