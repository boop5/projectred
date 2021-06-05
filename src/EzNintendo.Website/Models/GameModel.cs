using System.Linq;
using EzNintendo.Data;
using EzNintendo.Data.Nintendo;
using Microsoft.EntityFrameworkCore;

namespace EzNintendo.Website.Models
{
    public sealed class GameModel
    {
        public GameModel(ApplicationDbContext dbContext, long fsId)
        {
            // Game = dbContext.Games.Include(g => g.Trend).FirstOrDefault(g => g.FsId == fsId);
        }

        public Game Game { get; }
    }
}