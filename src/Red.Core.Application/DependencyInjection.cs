using System;
using System.Linq;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Behaviors;

namespace Red.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            var asses = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName?.StartsWith("Red") == true).ToList();
            services.AddValidatorsFromAssemblies(asses, ServiceLifetime.Transient);
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionLoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}