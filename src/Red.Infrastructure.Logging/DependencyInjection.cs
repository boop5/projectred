using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.Logging
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLoggingLayer(this IServiceCollection services)
        {
            services.AddLogging(o => o.AddDebug().AddConsole());
            services.AddTransient(typeof(IAppLogger<>), typeof(AppLogger<>));

            return services;
        }
    }
}