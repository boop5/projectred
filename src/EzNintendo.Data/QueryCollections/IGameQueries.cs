using System.Linq;
using EzNintendo.Data.Nintendo;

namespace EzNintendo.Data.QueryCollections
{
    public interface IGameQueries : IQueryCollection
    {
        IQueryable<Game> GetTrendRelevant();
    }
}