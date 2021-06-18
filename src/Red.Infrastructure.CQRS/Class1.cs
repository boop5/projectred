using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Common;

namespace Red.Infrastructure.CQRS
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCQRSLayer(this IServiceCollection services, params Assembly[] featureAssemblies)
        {
            services.AddMediatR(featureAssemblies);

            services.AddTransient<ICommandBus, CommandBus>();
            services.AddTransient<IQueryBus, QueryBus>();
            services.AddTransient<IEventBus, EventBus>();

            return services;
        }
    }

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

    internal sealed class CommandBus :  ICommandBus
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

    internal sealed class EventBus :  IEventBus
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
