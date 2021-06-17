using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Red.Core.Application.Common;
using Red.Infrastructure.CQRS;
using Red.Infrastructure.Logging;
using Red.Infrastructure.NintendoApi;
using Red.Infrastructure.Persistence;
using Red.Infrastructure.Spider;
using Red.Infrastructure.Spider.Worker;
using Red.Infrastructure.Utilities;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        (hostContext, services) =>
        {
            var cfg = hostContext.Configuration;

            services.AddEntityMerger();
            services.AddAppSettings(hostContext.Configuration);
            services.AddUtilities();
            services.AddLoggingLayer();
            services.AddPersistenceLayer(cfg.GetConnectionString("Default"));
            services.AddNintendoApi();
            services.AddCQRSLayer(typeof(ICommand).Assembly);

            services.AddHostedService<PriceSpider>();
            services.AddHostedService<LibrarySpider>();
            services.AddHostedService<MediaSpider>();
            services.AddHostedService<SalesSpider>();
        })
    .Build()

    .Run();
