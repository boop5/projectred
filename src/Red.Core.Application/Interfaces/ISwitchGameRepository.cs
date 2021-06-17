using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Interfaces
{
    public interface ISwitchGameRepository : IRepository<SwitchGame>
    {
        Task<SwitchGame?> GetByFsId(string fsId);
        Task<SwitchGame?> GetByNsuid(string nsuid);
        Task<IReadOnlyList<SwitchGame>> GetGamesForPriceQuery();
        Task<IReadOnlyList<SwitchGame>> GetGamesForMediaQuery(CultureInfo culture);
        Task<SwitchGame?> UpdateAsync(string productCode, Func<SwitchGame, SwitchGame> updateFunc);
    }
}