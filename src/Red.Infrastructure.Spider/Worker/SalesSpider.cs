using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Red.Core.Application.Extensions;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;
using Red.Infrastructure.Spider.Settings;

namespace Red.Infrastructure.Spider.Worker
{
    internal sealed class SalesSpider : Spider
    {
        private readonly IEshop _eshop;
        private readonly IServiceProvider _serviceProvider;

        public SalesSpider(IAppLogger<SalesSpider> log, 
                           SalesSpiderConfiguration configuration,
                           IServiceProvider serviceProvider, 
                           IEshop eshop)
            : base(log, configuration)
        {
            _serviceProvider = serviceProvider;
            _eshop = eshop;
        }

        private async Task<IReadOnlyCollection<EshopSalesQuery>> GetQueries()
        {
            // todo: use proper country/locale
            var culture = new CultureInfo("en-DE");

            // todo: move to constants
            var maxPerChunk = 30;
            var totalSales = await _eshop.GetTotalSales(culture);
            var queries = new List<EshopSalesQuery>();

            for (var i = 0; i < totalSales; i += maxPerChunk)
            {
                queries.Add(EshopSalesQuery.New(culture, i, maxPerChunk));
            }

            return queries;
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            var queries = await GetQueries();
            var tasks = queries.Select(ProcessQuery);

            await Task.WhenAll(tasks);
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
                updatedEntity = UpdateContentRating(updatedEntity, sale, query);

                if (!entity.Equals(updatedEntity))
                {
                    Log.LogInformation("Update {game} [{productCode}]", sale.Title, entity.ProductCode);
                    await repo.UpdateAsync(updatedEntity);
                }
            }
        }

        private SwitchGame UpdateColors(SwitchGame entity, SwitchGameSale sale)
        {
            if (!entity.Colors.SequenceEqual(sale.Colors))
            {
                return entity with {Colors = sale.Colors};
            }

            return entity;
        }

        private SwitchGame UpdateContentRating(SwitchGame entity, SwitchGameSale sale, EshopSalesQuery query)
        {
            var country = query.Culture.GetTwoLetterISORegionName();

            if (entity.ContentRating[country]?.Equals(sale.ContentRating) == false)
            {
                var newContentRating = entity.ContentRating.ToDictionary().ToDictionary(x => x.Key, x => x.Value);
                newContentRating[country] = sale.ContentRating;

                return entity with {ContentRating = CountryDictionary<ContentRating>.New(newContentRating)};
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
                        HeroBanner = new ImageDetail
                        {
                            Url = sale.HeroBannerUrl
                        }
                    }
                };
            }

            return entity;
        }
    }
}