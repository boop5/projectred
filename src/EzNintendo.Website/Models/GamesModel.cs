using System.Collections.Generic;
using System.Linq;
using EzNintendo.Data;
using EzNintendo.Data.Nintendo;

namespace EzNintendo.Website.Models
{
    public sealed class GamesModel
    {
        public GamesModel(ApplicationDbContext dbContext)
        {
            //Games = dbContext.Games
            //                 .OrderByDescending(g => g.Created)
            //                 .Select(g => new Game
            //                 {
            //                     FsId = g.FsId,
            //                     Title = g.Title
            //                 })
            //                 .ToList();
        }

        public IEnumerable<Game> Games { get; }
    }
}