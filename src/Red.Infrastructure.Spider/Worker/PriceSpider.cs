using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Red.Core.Application;

namespace Red.Infrastructure.Spider.Worker
{
    public class PriceSpider : ScheduledWorker
    {
        public PriceSpider(ILogger<PriceSpider> log) 
            :base(log)
        {
        }

        protected override Task LoopAsync(CancellationToken stoppingToken = default)
        {
            return Task.CompletedTask;
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }
    }
}