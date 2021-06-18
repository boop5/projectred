using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Red.Core.Application.Common;

namespace Red.Infrastructure.CQRS
{
    internal sealed class CommandBus : ICommandBus
    {
        private readonly IMediator _mediator;

        public CommandBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Send(ICommand command, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(command, cancellationToken);
        }

        public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}