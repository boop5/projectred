using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Red.Core.Application;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider.Worker
{
    public class PriceSpider : TimedWorker
    {
        private readonly IEshop _eshop;
        private readonly ISwitchGameRepository _repo;

        public PriceSpider(IAppLogger<PriceSpider> log, IEshop eshop, ISwitchGameRepository repo)
            : base(log)
        {
            _eshop = eshop;
            _repo = repo;
        }

        protected override TimeSpan GetInitialDelay()
        {
            return TimeSpan.FromMinutes(0);
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            var games = await _repo.Get()
                                   .Select(
                                       x => new SwitchGame
                                       {
                                           Nsuids = x.Nsuids,
                                           ProductCode = x.ProductCode,
                                           Region = x.Region
                                       })
                                   .ToListAsync(stoppingToken);

            var nsuids = games.Where(x => x.Nsuids.Count == 1).Select(x => x.Nsuids[0]);

            var prices = await _eshop.GetPrices(new EshopMultiPriceQuery(nsuids));

            ;
        }
    }
}