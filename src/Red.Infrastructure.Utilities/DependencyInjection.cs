using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.Utilities
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUtilities(this IServiceCollection services)
        {
            services.AddTransient<ISlugBuilder, SlugBuilder>();
            services.AddTransient<IEshopSlugBuilder, EshopSlugBuilder>();

            return services;
        }
    }
}