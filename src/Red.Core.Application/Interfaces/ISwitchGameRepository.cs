using System.Threading.Tasks;
using Red.Core.Domain.Models;

namespace Red.Core.Application.Interfaces
{
    public interface ISwitchGameRepository : IRepository<SwitchGame>
    {
        Task<SwitchGame> GetByProductCode(string productCode);
    }
}