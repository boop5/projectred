using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Red.Infrastructure.Spider;

Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<LibrarySpider>();
        })
        .Build()
        .Run();