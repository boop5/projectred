using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Red.Core.Application.Common
{
    public interface ICommand : IRequest{}
    public interface IEvent : IRequest{}
    public interface ICommand<out TResponse> : IRequest<TResponse>{}
    public interface IQuery<out TResponse> : IRequest<TResponse>{}

    public interface IQueryBus
    {
        public Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    }

    public interface ICommandBus
    {
        public Task Send(ICommand command, CancellationToken cancellationToken = default);
        public Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
    }

    public interface IEventBus
    {
        public Task Raise(IEvent @event, CancellationToken cancellationToken = default);
    }
}
