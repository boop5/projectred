using System.Collections.Generic;
using System.Threading.Tasks;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Interfaces
{
    public interface IEshop
    {
        Task<IReadOnlyCollection<SwitchGame>> SearchGames(EshopGameQuery query);
        Task<IReadOnlyCollection<SwitchGamePrice>> GetPrices(EshopMultiPriceQuery query);
        Task<int> GetTotalGames();
    }
}