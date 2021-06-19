using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Red.Core.Application.Interfaces;

namespace Red.Core.Application.Behaviors
{
    internal sealed class ExceptionLoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        private IAppLogger<LoggingBehavior<TRequest, TResponse>> Log { get; }

        public ExceptionLoggingBehavior(IAppLogger<LoggingBehavior<TRequest, TResponse>> log)
        {
            Log = log;
        }

        public async Task<TResponse> Handle(TRequest request,
                                            CancellationToken cancellationToken,
                                            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (ValidationException e)
            {
                Log.LogWarning("Validation failed in request '{request}'. Errors: {@errors}", typeof(TRequest).Name, e.Errors);
                throw;
            }
            catch (Exception e)
            {
                Log.LogWarning(e, "Exception occurred in request '{request}", typeof(TRequest).Name);
                throw;
            }
        }
    }
}