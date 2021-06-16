using Red.Core.Domain.Models;

namespace Red.Core.Application.Interfaces
{
    public interface IEntityMerger<T> where T : class
    {
        T Merge(T targetEntity, T sourceEntity);
    }

    public interface ISwitchGameMerger : IEntityMerger<SwitchGame>
    {

    }
}
