using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Interfaces;
using Red.Infrastructure.Spider.Settings;

namespace Red.Infrastructure.Spider
{
    internal static class DependencyInjection
    {
        public static IServiceCollection AddEntityMerger(this IServiceCollection services)
        {
            services.AddTransient<ISwitchGameMerger, SwitchGameMerger>();

            return services;
        }

        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);

            services.AddSingleton(appSettings);
            services.AddSingleton(appSettings.Workers);
            services.AddSingleton(appSettings.Workers.LibrarySpider);
            services.AddSingleton(appSettings.Workers.MediaSpider);
            services.AddSingleton(appSettings.Workers.PriceSpider);
            services.AddSingleton(appSettings.Workers.SalesSpider);

            return services;
        }
    }
}