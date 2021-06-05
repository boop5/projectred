using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using EzNintendo.Common.Utilities;
using EzNintendo.Data;
using EzNintendo.Data.QueryCollections;
using EzNintendo.Website.Services.Background;
using EzNintendo.Website.Services.Data;
using EzNintendo.Website.Services.Mail;
using EzNintendo.Website.Services.Media;
using EzNintendo.Website.Services.Nintendo;
using EzNintendo.Website.Services.Web;
using EzNintendo.Website.Shop;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace EzNintendo.Website
{
    public sealed class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private ILogger _log;

        /// <summary>
        ///     Initializes a new instance of the <see bf="Startup" /> class.
        /// </summary>
        /// <param name="configuration">Application Configurations.</param>
        /// <param name="environment">The <see cref="IWebHost" /> environment.</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            _configuration = configuration;

            //_configuration  = new ConfigurationBuilder()
            //                      .SetBasePath(environment.ContentRootPath)
            //                      .AddJsonFile("appsettings.json", false, true)
            //                      .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", false, true)
            //                      .AddEnvironmentVariables(prefix: "")
            //                      .Build();
        }

        /// <summary>
        ///     This method gets called by the runtime. Used to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Called by runtime.")]
        public void ConfigureServices(IServiceCollection services)
        {
            // Database
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // Identity
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddDefaultUI()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            // Frontend
            var mvcBuilder = services.AddControllersWithViews();
            if (_environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            // Custom Services
            services.AddTransient<NLogLoggerFactory>();
            services.AddTransient<IEmailSender, SmtpMailSender>();
            services.AddTransient<eShopApi>();
            services.AddTransient<HttpService>();
            services.AddTransient<ThrowHelper>();
            services.AddTransient<ImageService>();
            services.AddTransient<TrendService>();
            services.AddTransient<IGameQueries,GameQueries>();
            services.AddSingleton<CalendarService>();
            services.AddSingleton<ApplicationDbContextFactory>();

            services.AddTransient<IFileSystem>(_ => new FileSystem());

            // todo: move to EzNintendo.Nintendo.API Assembly or so
            services.AddSingleton<GameSearchQueryBuilder>();

            services.AddHostedService<UpdateGameLibraryBackgroundService>();
            services.AddHostedService<UpdateTrendBackgroundService>();
        }

        /// <summary>
        ///     This method gets called by the runtime. Used to configure the HTTP request pipeline.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Called by runtime.")]
        public void Configure(IApplicationBuilder app,
                              ApplicationDbContext dbContext,
                              ILogger<Startup> log,
                              IHostApplicationLifetime applicationLifetime)
        {
            _log = log;
            _log.LogDebug("Start to Configure the WebApp.");

            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                _log.LogCritical(e, "Failed to migrate Database.");
                applicationLifetime.StopApplication();
            }

            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //var imageConfiguration = _configuration.Get<ImageConfiguration>();
            app.UseStaticFiles(new StaticFileOptions
            {
                // todo: use _configuration.Get...
                //FileProvider = new PhysicalFileProvider(imageConfiguration.BasePath, ExclusionFilters.None),
                FileProvider = new PhysicalFileProvider("c:\\temp\\eznintendo\\pics", ExclusionFilters.None),
                RequestPath = "/static"
            });
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("games",
                                             "games",
                                             new
                                             {
                                                 controller = "Games",
                                                 action = "Index"
                                             });
                endpoints.MapControllerRoute("games",
                                             "games/{id}",
                                             new
                                             {
                                                 controller = "Games",
                                                 action = "GetGameById"
                                             });
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                // endpoints.MapHub<LogHub>("/admin/hubs/log");
            });
        }
    }
}