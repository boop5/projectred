using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Red.Core.Application.Common;

namespace Red.Infrastructure.CQRS
{
    internal sealed class QueryBus : IQueryBus
    {
        private readonly IMediator _mediator;

        public QueryBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(query, cancellationToken);
        }
    }
}