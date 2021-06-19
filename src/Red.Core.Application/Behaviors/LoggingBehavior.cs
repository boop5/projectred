using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Red.Core.Application.Interfaces;

namespace Red.Core.Application.Behaviors
{
    internal sealed class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        private IAppLogger<LoggingBehavior<TRequest, TResponse>> Log { get; }

        public LoggingBehavior(IAppLogger<LoggingBehavior<TRequest, TResponse>> log)
        {
            Log = log;
        }

        public async Task<TResponse> Handle(TRequest request,
                                            CancellationToken cancellationToken,
                                            RequestHandlerDelegate<TResponse> next)
        {
            Log.LogDebug("Handling {requestType}", typeof(TRequest).Name);
            var watch = Stopwatch.StartNew();

            var response = await next();

            watch.Stop();
            Log.LogDebug(
                "Handled {responseType} in {elapsed}s",
                typeof(TRequest).Name,
                watch.Elapsed.TotalSeconds.ToString("F"));

            return response;
        }
    }
}