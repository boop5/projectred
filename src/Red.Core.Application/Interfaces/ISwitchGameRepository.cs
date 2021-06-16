using System;
using System.Globalization;
using System.Threading.Tasks;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Interfaces
{
    public interface ISwitchGameRepository : IRepository<SwitchGame>
    {
        Task<SwitchGame?> GetMatchingGame(SwitchGame game, CultureInfo culture);
        Task<SwitchGame?> GetByFsId(string fsId);
        Task<SwitchGame?> GetByNsuid(string nsuid);
        Task<SwitchGame?> UpdateAsync(string productCode, Func<SwitchGame, SwitchGame> updateFunc);
    }
}