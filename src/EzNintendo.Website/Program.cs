using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Web;

namespace EzNintendo.Website
{
    public static class Program
    {
        /// <summary>
        ///     Entry point of the application.
        /// </summary>
        /// <param name="args">The commandline arguments.</param>
        public static async Task Main(string[] args)
        {
            LogManager.ThrowConfigExceptions = true;
            LogManager.ThrowExceptions = true;
            LogManager.Configuration.Install(new InstallationContext {IgnoreFailures = false});

            var log = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                log.Info("Startup");
                var builder = CreateHostBuilder(args);
                var host = builder.Build();

                await host.RunAsync();
            }
            catch (Exception e)
            {
                log.Error(e, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        /// <summary>
        ///     Prepares an instance of <see cref="IHost" /> by using <see cref="IHostBuilder" />.
        /// </summary>
        /// <param name="args">The commandline arguments.</param>
        /// <returns>Configured <see cref="IWebHostBuilder" /> to use <see cref="Startup" /> as Startup.</returns>
        /// <remarks>
        ///     If the app uses Entity Framework Core, don't change the name or signature of the CreateHostBuilder method.
        ///     The Entity Framework Core tools expect to find a CreateHostBuilder method that configures the host without running
        ///     the app. For more information, see Design-time DbContext Creation.
        ///     https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
        /// </remarks>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Used by EF Tools")]
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            var host = builder.ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseStartup<Startup>()
                              .ConfigureLogging(log => log.ClearProviders())
                              .UseNLog();
            });

            return host;
        }
    }
}