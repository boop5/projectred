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
        private readonly WorkerSettings _configuration;
        private readonly IServiceProvider _serviceProvider;

        public SalesSpider(IAppLogger<SalesSpider> log,
                           WorkerSettings configuration,
                           IServiceProvider serviceProvider,
                           IEshop eshop)
            : base(log, configuration.SalesSpider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _eshop = eshop;
        }

        private async Task<IReadOnlyCollection<EshopSalesQuery>> GetQueries(CultureInfo culture)
        {
            var queries = new List<EshopSalesQuery>();
            // todo: move to constants
            var maxPerChunk = 30;
            var totalSales = await _eshop.GetTotalSales(culture);

            for (var i = 0; i < totalSales; i += maxPerChunk)
            {
                queries.Add(EshopSalesQuery.New(culture, i, maxPerChunk));
            }

            return queries;
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            foreach (var culture in _configuration.Cultures)
            {
                var queries = await GetQueries(culture);
                var tasks = queries.Select(x => ProcessQuery(culture, x));
                await Task.WhenAll(tasks);
            }
        }

        private async Task ProcessQuery(CultureInfo culture, EshopSalesQuery query)
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
                updatedEntity = UpdateHeroBanner(culture, updatedEntity, sale);
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
                return entity with { Colors = sale.Colors };
            }

            return entity;
        }

        private SwitchGame UpdateContentRating(SwitchGame entity, SwitchGameSale sale, EshopSalesQuery query)
        {
            var region = query.Culture.GetTwoLetterISORegionName();

            if (entity.ContentRating[region]?.Equals(sale.ContentRating) == false)
            {
                var newContentRating = entity.ContentRating.Merge(entity.ContentRating);
                newContentRating[region] = sale.ContentRating;

                return entity with { ContentRating = newContentRating };
            }

            return entity;
        }

        private SwitchGame UpdateHeroBanner(CultureInfo culture, SwitchGame entity, SwitchGameSale sale)
        {
            var region = culture.GetTwoLetterISORegionName();
            var lang = culture.TwoLetterISOLanguageName;

            if (!string.IsNullOrWhiteSpace(sale.HeroBannerUrl)
                && !string.Equals(entity.Media[lang]?.HeroBanner?.Url, sale.HeroBannerUrl))
            {
                var media = entity.Media.Merge(entity.Media); // clone
                media[region] ??= new SwitchGameMedia();
                media[region] = media[region]! with
                {
                    HeroBanner = new ImageDetail { Url = sale.HeroBannerUrl }
                };

                return entity with { Media = media };
            }

            return entity;
        }
    }
}