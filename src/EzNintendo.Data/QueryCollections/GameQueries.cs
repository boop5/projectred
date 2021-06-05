using System.Linq;
using EzNintendo.Data.Nintendo;

namespace EzNintendo.Data.QueryCollections
{
    public sealed class GameQueries : QueryCollectionBase, IGameQueries
    {
        public IQueryable<Game> GetTrendRelevant()
        {
            return Ctx.Games
                      .Select(g => new Game
                      {
                          Id = g.Id,
                          Title = g.Title,
                          NsUid_EU = g.NsUid_EU,
                          NsUid_JP = g.NsUid_JP,
                          NsUid_US = g.NsUid_US,
                          Trend = g.Trend
                      });
        }
    }
}