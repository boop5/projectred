using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.Persistence
{
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextFactory<LibraryContext>(o => BuildDbContextOptions(connectionString, o));
            services.AddDbContext<LibraryContext>(o => BuildDbContextOptions(connectionString, o));

            services.AddTransient<ISwitchGameRepository, SwitchGameRepository>();

            using var ctx = services.BuildServiceProvider().GetRequiredService<LibraryContext>();
            ctx.Database.Migrate();

            return services;
        }

        private static DbContextOptionsBuilder BuildDbContextOptions(string connectionString, DbContextOptionsBuilder builder)
        {
            return builder.UseSqlServer(connectionString, BuildSqlServerOptions);
        }

        private static void BuildSqlServerOptions(SqlServerDbContextOptionsBuilder obj)
        {
            obj.EnableRetryOnFailure(360, TimeSpan.FromSeconds(10), null);
        }
    }
}