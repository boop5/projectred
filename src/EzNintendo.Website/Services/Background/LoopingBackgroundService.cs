using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Website.Services.Background
{
    public abstract class LoopingBackgroundService : BackgroundService
    {
        private readonly TimeSpan _minTaskDelay = TimeSpan.FromSeconds(5);

        private int _iteration;
        private TimeSpan _initialDelay;
        private TimeSpan _taskInterval;

        protected LoopingBackgroundService(ILogger log)
        {
            Log = log;

            Log.LogTrace("Instance created.");
        }

        protected ILogger Log { get; }

        protected abstract TimeSpan GetTaskInterval();

        protected virtual TimeSpan GetInitialDelay()
        {
            return TimeSpan.Zero;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.LogInformation("Start BackgroundService");
            stoppingToken.Register(() => Log.LogDebug("Token is cancelled."));

            _initialDelay = GetInitialDelay();
            _taskInterval = GetTaskInterval();

            if (_taskInterval < _minTaskDelay)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_iteration == default)
            {
                await Task.Delay(_initialDelay, stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                _iteration++;
                Log.LogDebug("Run BackgroundTask for the {n}. time.", _iteration);

                var started = DateTime.UtcNow;
                try
                {
                    await LoopAsync(stoppingToken);
                }
                catch (Exception e)
                {
                    Log.LogWarning(e, "Failed to run BackgroundService!");
                }

                Log.LogDebug("Finished Loop.");

                var duration = DateTime.UtcNow - started;
                var interval = _taskInterval - duration;

                if (interval < TimeSpan.Zero)
                {
                    interval = TimeSpan.FromMinutes(1);
                }

                // wait for next iteration#
                await Task.Delay(interval, stoppingToken);
            }

            Log.LogInformation("Stop BackgroundService.");
        }

        protected abstract Task LoopAsync(CancellationToken stoppingToken = default);
    }
}