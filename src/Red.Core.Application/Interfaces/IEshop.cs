using System.Collections.Generic;
using System.Threading.Tasks;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Interfaces
{
    public interface IEshop
    {
        // todo: GetDlc()
        /*
           https://searching.nintendo-europe.com/de/select
           ?q=*
           &fq=type:DLC AND sorting_title:* AND *:*
           &sort=deprioritise_b asc, popularity asc
           &start=0
           &rows=24
           &wt=json
           &bf=linear(ms(priority,NOW%2FHOUR),1.1e-11,0)
           &bq=!deprioritise_b:true^999
           &json.wrf=nindo.net.jsonp.jsonpCallback_22244_80000000447
         */
        Task<IReadOnlyCollection<SwitchGame>> SearchGames(EshopGameQuery query);
        Task<IReadOnlyCollection<SwitchGamePrice>> GetPrices(EshopMultiPriceQuery query);
        Task<int> GetTotalGames();
    }
}