using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Red.Infrastructure.Spider.Worker;

Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<PriceSpider>();
            services.AddHostedService<LibrarySpider>();
        })
        .Build()
        .Run();