using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Red.Infrastructure.NintendoApi;
using Red.Infrastructure.Persistence;
using Red.Infrastructure.Spider.Worker;
using Red.Infrastructure.Utilities;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        (hostContext, services) =>
        {
            var cfg = hostContext.Configuration;

            services.AddUtilities();
            services.AddPersistenceLayer(cfg.GetConnectionString("Default"));
            services.AddNintendoApi();

            services.AddHostedService<PriceSpider>();
            services.AddHostedService<LibrarySpider>();
        })
    .Build()
    .Run();