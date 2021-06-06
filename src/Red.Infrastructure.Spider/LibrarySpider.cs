using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Red.Core.Application;

namespace Red.Infrastructure.Spider
{
    public class LibrarySpider : ScheduledWorker
    {
        public LibrarySpider(ILogger<LibrarySpider> log) 
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
