using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.Persistence
{
    internal sealed class DbInitializer
    {
        private IAppLogger<DbInitializer> Log { get; }
        private readonly IServiceProvider _serviceProvider;

        public DbInitializer(IAppLogger<DbInitializer> log, IServiceProvider serviceProvider)
        {
            Log = log;
            _serviceProvider = serviceProvider;
        }

        public void Init()
        {
            try
            {
                using LibraryContext ctx = _serviceProvider.GetRequiredService<LibraryContext>();

                if (ctx.Database.GetPendingMigrations().Any())
                {
                    ctx.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                Log.LogCritical(e, "Failed to initialize database");
            }
        }
    }
}