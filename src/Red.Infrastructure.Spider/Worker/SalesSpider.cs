using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider.Worker
{
    public class SalesSpider : TimedWorker
    {
        private readonly IEshop _eshop;
        private readonly IServiceProvider _serviceProvider;

        public SalesSpider(IAppLogger<SalesSpider> log,
                           IServiceProvider serviceProvider,
                           IEshop eshop)
            : base(log)
        {
            _serviceProvider = serviceProvider;
            _eshop = eshop;
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            // todo: use proper country/locale
            var country = "DE";
            var locale = "de";
            // todo: move to constants
            var maxPerChunk = 30;
            var totalSales = await _eshop.GetTotalSales();

            for (var i = 0; i < totalSales; i += maxPerChunk)
            {
                var repo = _serviceProvider.GetRequiredService<ISwitchGameRepository>();
                var query = EshopSalesQuery.New(country, locale, i, maxPerChunk);
                var response = await _eshop.GetSales(query);

                foreach (var sale in response)
                {
                    // todo: take care of region
                    var entities = await repo.Get().Select(x => new {x.Nsuids, x.ProductCode, x.Colors}).ToListAsync(stoppingToken);
                    entities = entities.Where(x => x.Nsuids.Contains(sale.Nsuid)).ToList();

                    if (entities.Count != 1)
                    {
                        continue;
                    }

                    var entity = entities[0];
                    if (!entity.Colors.Equals(sale.Colors))
                    {
                        Log.LogInformation("Update colors for {game} [{nsuid}]", sale.Title, sale.Nsuid);

                        await repo.UpdateAsync(entity.ProductCode, x => x with {Colors = sale.Colors});
                    }
                }
            }
        }
    }
}