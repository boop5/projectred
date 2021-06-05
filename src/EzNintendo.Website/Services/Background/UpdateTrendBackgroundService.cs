using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using EzNintendo.Domain.eShop;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Website.Services.Background
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Runtime creates Instance.")]
    public sealed class UpdateTrendBackgroundService : LoopingBackgroundService
    {
        private readonly TrendService _trendService;

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter", Justification = "ILogger needs to be generic for DI")]
        public UpdateTrendBackgroundService(ILogger<UpdateTrendBackgroundService> log,
                                            TrendService trendService)
            : base(log)
        {
            _trendService = trendService;
        }

        protected override TimeSpan GetInitialDelay()
        {
            return TimeSpan.FromHours(0);
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(60);
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            foreach (eShopRegion region in Enum.GetValues(typeof(eShopRegion)))
            {
                // todo: do not use europe only :angery:
                if (region != eShopRegion.Europe)
                {
                    Log.LogInformation("Skip {region}", region);
                    continue;
                }

                Log.LogInformation("Update {region}", region);

                await _trendService.UpdateRegion(region, stoppingToken);

                // todo: log "Finished Updating Region in xxx seconds"
            }

            Log.LogInformation("Finished all Updates."); // todo: in xxx seconds
        }
    }
}