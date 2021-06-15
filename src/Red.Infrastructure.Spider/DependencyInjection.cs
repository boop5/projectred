using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Red.Infrastructure.Spider
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);

            var defaultDelay = 60;
            var defaultInterval = 60;
            services.AddSingleton(new LibrarySpiderConfiguration
            {
                Interval = appSettings.Workers["LibrarySpider"]?.Interval ?? defaultInterval,
                Delay = appSettings.Workers["LibrarySpider"]?.Delay ?? defaultDelay
            });

            services.AddSingleton(new PriceSpiderConfiguration
            {
                Interval = appSettings.Workers["PriceSpider"]?.Interval ?? defaultInterval,
                Delay = appSettings.Workers["PriceSpider"]?.Delay ?? defaultDelay
            });

            services.AddSingleton(new SalesSpiderConfiguration
            {
                Interval = appSettings.Workers["SalesSpider"]?.Interval ?? defaultInterval,
                Delay = appSettings.Workers["SalesSpider"]?.Delay ?? defaultDelay
            });

            services.AddSingleton(new MediaSpiderConfiguration
            {
                Interval = appSettings.Workers["MediaSpider"]?.Interval ?? defaultInterval,
                Delay = appSettings.Workers["MediaSpider"]?.Delay ?? defaultDelay
            });

            return services;
        }
    }
}