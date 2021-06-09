using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.NintendoApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNintendoApi(this IServiceCollection services)
        {
            services.AddTransient<EshopHttpClient>();
            services.AddTransient<EshopConverter>();
            services.AddTransient<EshopUrlBuilder>();
            services.AddTransient<IEshop, Eshop>();

            return services;
        }
    }
}