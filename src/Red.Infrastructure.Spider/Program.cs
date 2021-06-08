using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Red.Infrastructure.NintendoApi;
using Red.Infrastructure.Persistence;
using Red.Infrastructure.Spider.Worker;

Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddPersistenceLayer("Server=lsc.pw,7701;Database=eshop;User Id=sa;Password=QwWUmu!kjC9mu4ZFdC4;");
            services.AddNintendoApi();

            services.AddHostedService<PriceSpider>();
            services.AddHostedService<LibrarySpider>();
        })
        .Build()
        .Run();