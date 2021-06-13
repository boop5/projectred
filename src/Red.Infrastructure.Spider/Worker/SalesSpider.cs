using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Red.Core.Application;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider.Worker
{
    public class SalesSpider : TimedWorker
    {
        private readonly IEshop _eshop;

        public SalesSpider(IAppLogger<SalesSpider> log,
                           IEshop eshop) 
            : base(log)
        {
            _eshop = eshop;
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            // todo: use proper country/locale
            var country = "DE";
            var locale = "de";
            // todo: move to constants
            var maxPerChunk = 30;
            var totalSales = await _eshop.GetTotalSales();

            var html = "<html><head><style>html{background-color:#fff;color:#000}@media (prefers-color-scheme:dark){html{background-color:#0e0e0e;color:#fff}}div .container{margin-bottom:40px}div .item{height:80px;width:80px;margin-bottom:40px}ul{list-style-type:none;margin:0;padding:0;overflow:hidden}li{float:left;margin-right:8px}</style></head><body>";

            for (var i = 0; i < totalSales; i += maxPerChunk)
            {
                var query = EshopSalesQuery.New(country, locale, start: i, count: maxPerChunk);

                var response = await _eshop.GetSales(query);

                foreach (var sale in response)
                {
                    html += $"<div class=container><p>{sale.Title} [{sale.Nsuid}]</p><ul>";

                    foreach (var color in sale.Colors)
                    {
                        html += $"<li><div class=item style='background-color:{color.HexCode}'>{color.HexCode}</div></li>";
                    }

                    html += "</ul></div>";
                }
            }

            html += "</body></html>";

            await System.IO.File.WriteAllTextAsync($"c:\\temp\\{DateTime.Now:HHmmss}.html", html);

            ;
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }
    }
}
