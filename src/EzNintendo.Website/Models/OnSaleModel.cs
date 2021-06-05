using System;
using System.Collections.Generic;
using System.Linq;
using EzNintendo.Data.Nintendo;
using EzNintendo.Website.Services.Data;

namespace EzNintendo.Website.Models
{
    public sealed class OnSaleModel
    {
        public OnSaleModel(ApplicationDbContextFactory ctxFactory)
        {
            throw new NotImplementedException();
            //using var ctx = ctxFactory.Get();

            //Games = (from g in ctx.Games
            //                        from p in ctx.PriceHistory
            //                                     // join game
            //                                     .Where(p => p.GameId == g.Id &&
            //                                                 p.Discount)
            //                                     .OrderByDescending(p => p.Created)
            //                                     .Take(1)
            //                        where
            //                            g.Nsuid != default
            //                        // ** DO NOT CHANGE THE SELECT. **
            //                        // We have to return both items to query the History.
            //                        // Otherwise Game.History gonna be null
            //                        select g)
            //                        //select new OnSaleGameModel(g))
            //    .ToList();
        }

        public IEnumerable<Game> Games { get; }
    }
}