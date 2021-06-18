using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Red.Core.Application.Common;

namespace Red.Infrastructure.CQRS
{
    internal sealed class EventBus : IEventBus
    {
        private readonly IMediator _mediator;

        public EventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Raise(IEvent @event, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(@event, cancellationToken);
        }
    }
}