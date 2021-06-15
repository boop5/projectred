using System;
using Red.Core.Application;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.Spider.Worker
{
    internal abstract class Spider : TimedWorker
    {
        protected SpiderConfiguration Configuration { get; }

        protected Spider(IAppLogger log, SpiderConfiguration configuration)
            : base(log)
        {
            Configuration = configuration;

            log.LogDebug("Initialize {spider} with configuration [delay: {delay}min, interval: {interval}min] ", 
                         GetType().Name, Configuration.Delay, Configuration.Interval);
        }

        protected sealed override TimeSpan GetInitialDelay()
        {
            return TimeSpan.FromMinutes(Configuration.Delay);
        }

        protected sealed override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(Configuration.Interval);
        }
    }
}