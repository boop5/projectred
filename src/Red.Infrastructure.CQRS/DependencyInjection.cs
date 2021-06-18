using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Common;

namespace Red.Infrastructure.CQRS
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCQRSLayer(this IServiceCollection services, params Assembly[] featureAssemblies)
        {
            services.AddMediatR(featureAssemblies);

            services.AddTransient<ICommandBus, CommandBus>();
            services.AddTransient<IQueryBus, QueryBus>();
            services.AddTransient<IEventBus, EventBus>();

            return services;
        }
    }
}