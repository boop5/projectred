using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Red.Core.Application
{
    public abstract class ScheduledWorker : BackgroundService
    {
        private TimeSpan _initialDelay;
        private TimeSpan _taskInterval;

        protected ILogger Log { get; }

        protected ScheduledWorker(ILogger log)
        {
            Log = log;

            Log.LogTrace("Worker created");
        }

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.LogInformation("Start worker");
            SetupWorker(stoppingToken);

            await Task.Delay(_initialDelay, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                var started = DateTime.UtcNow;
                Log.LogDebug("Start execution");

                try
                {
                    await LoopAsync(stoppingToken);
                }
                catch (Exception e)
                {
                    var duration = DateTime.UtcNow - started;
                    Log.LogWarning(e, $"Failed to execute worker after {duration:g}");
                }
                finally
                {
                    var duration = DateTime.UtcNow - started;
                    var interval = TimeSpan.Zero;

                    if (_taskInterval > duration)
                    {
                        interval = _taskInterval - duration;
                    }

                    Log.LogDebug($"Stop execution after {duration:g}");

                    // wait for next iteration
                    Log.LogDebug("Next execution in {time}", interval.ToString("g"));
                    await Task.Delay(interval, stoppingToken);
                }
            }
        }

        /// <summary>
        ///     Gets a value which determines how much time has to pass before the worker gets executed for the first time. <br />
        ///     Default is <see cref="TimeSpan.Zero" />.
        /// </summary>
        /// <remarks>Can be overriden.</remarks>
        /// <returns></returns>
        protected virtual TimeSpan GetInitialDelay()
        {
            return TimeSpan.Zero;
        }

        /// <summary>
        ///     Gets a value which determines how often the worker gets executed. <br />
        ///     Default is <see cref="TimeSpan.Zero" />.
        /// </summary>
        /// <remarks>Can be overriden.</remarks>
        /// <returns>
        ///     A <see cref="TimeSpan" /> which determines the time that has to be between 2 executions.
        /// </returns>
        protected virtual TimeSpan GetTaskInterval()
        {
            return TimeSpan.Zero;
        }

        protected abstract Task LoopAsync(CancellationToken stoppingToken = default);

        private void SetupWorker(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => Log.LogDebug("Worker stopped"));

            _initialDelay = GetInitialDelay();
            _taskInterval = GetTaskInterval();
        }
    }
}