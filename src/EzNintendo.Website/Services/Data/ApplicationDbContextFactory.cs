using System;
using System.Diagnostics.CodeAnalysis;
using EzNintendo.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Website.Services.Data
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Runtime creates Instance.")]
    public sealed class ApplicationDbContextFactory
    {
        private readonly ILogger<ApplicationDbContextFactory> _log;
        private readonly IServiceScopeFactory _scopeFactory;

        public ApplicationDbContextFactory(ILogger<ApplicationDbContextFactory> log,
                                           IServiceScopeFactory scopeFactory)
        {
            _log = log;
            _scopeFactory = scopeFactory;
        }

        public ApplicationDbContext Get()
        {
            _log.LogDebug("Create new DbContext.");

            try
            {
                var serviceProvider = _scopeFactory.CreateScope().ServiceProvider;
                var context = serviceProvider.GetService<ApplicationDbContext>();

                return context;
            }
            catch (Exception e)
            {
                _log.LogWarning(e, "Failed to create DbContext.");
                throw;
            }
        }
    }
}