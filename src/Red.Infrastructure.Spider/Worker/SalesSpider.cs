using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public SalesSpider(IAppLogger<SalesSpider> log, IServiceProvider serviceProvider, IEshop eshop)
            : base(log)
        {
            _serviceProvider = serviceProvider;
            _eshop = eshop;
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }

        protected override TimeSpan GetInitialDelay()
        {
            return TimeSpan.FromMinutes(0);
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            var queries = await GetQueries();
            var tasks = queries.Select(ProcessQuery);

            await Task.WhenAll(tasks);
        }

        private async Task<IReadOnlyCollection<EshopSalesQuery>> GetQueries()
        {
            // todo: use proper country/locale
            var country = "DE";
            var locale = "de";

            // todo: move to constants
            var maxPerChunk = 30;
            var totalSales = await _eshop.GetTotalSales();
            var queries = new List<EshopSalesQuery>();

            for (var i = 0; i < totalSales; i += maxPerChunk)
            {
                queries.Add(EshopSalesQuery.New(country, locale, i, maxPerChunk));
            }

            return queries;
        }

        private async Task ProcessQuery(EshopSalesQuery query)
        {
            var repo = _serviceProvider.GetRequiredService<ISwitchGameRepository>();
            var response = await _eshop.GetSales(query);

            foreach (var sale in response)
            {
                // todo: take care of region
                var entity = await repo.GetByNsuid(sale.Nsuid);
                if (entity == null)
                {
                    Log.LogWarning("Game not found {game} [{nsuid}]", sale.Title, sale.Nsuid);
                    continue;
                }

                var updatedEntity = entity with { };
                updatedEntity = UpdateColors(updatedEntity, sale);
                updatedEntity = UpdateHeroBanner(updatedEntity, sale);
                updatedEntity = UpdateAgeRating(updatedEntity, sale);

                if (!entity.Equals(updatedEntity))
                {
                    Log.LogInformation("Update {game}", sale.Title);
                    await repo.UpdateAsync(entity);
                }
            }
        }

        private SwitchGame UpdateAgeRating(SwitchGame entity, SwitchGameSale sale)
        {
            // todo: gotta save age rating per country. USK is a german rating, other countries have other ratings.....
            return entity;

            if (sale.UskRating.HasValue && !Nullable.Equals(entity.AgeRating, sale.UskRating))
            {
                return entity with
                {
                    AgeRating = sale.UskRating
                };
            }

            return entity;
        }

        private SwitchGame UpdateHeroBanner(SwitchGame entity, SwitchGameSale sale)
        {
            if (!string.IsNullOrWhiteSpace(sale.HeroBannerUrl) 
                && !string.Equals(entity.Media.HeroBanner?.Url, sale.HeroBannerUrl))
            {
                return entity with
                {
                    Media = entity.Media with
                    {
                        HeroBanner = new ImageDetail()
                        {
                            Url = sale.HeroBannerUrl
                        }
                    }
                };
            }

            return entity;
        }

        private SwitchGame UpdateColors(SwitchGame entity, SwitchGameSale sale)
        {
            if (!entity.Colors.SequenceEqual(sale.Colors))
            {
                return entity with { Colors = sale.Colors };
            }

            return entity;
        }
    }
}