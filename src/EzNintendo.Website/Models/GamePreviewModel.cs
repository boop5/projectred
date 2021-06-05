using System.Collections.Generic;
using System.Linq;
using EzNintendo.Data.Nintendo;
using EzNintendo.Website.Services.Nintendo;

namespace EzNintendo.Website.Models
{
    public sealed class GamePreviewModel
    {
        public GamePreviewModel(Game game)
        {
            var trend = game.Trend.OrderBy(t => t.Created).ToList();
            var currentPrice = trend.LastOrDefault();

            if (currentPrice != null)
            {
                Price = currentPrice.Price;
                Currency = "€"; // todo: use actual currency
                Discount = -666; // DiscountHelper.CalculateDiscount((float) game.RegularPrice, currentPrice.Price);
                Note = CreateNote(game, trend, currentPrice);
            }

            Title = game.Title;
            //Cover = $"/static/{game.FsId}/images/square.min.jpg";
        }

        public string Title { get; }
        public string Note { get; }
        public float? Price { get; }
        public string Currency { get; }
        public int Discount { get; }
        public string Cover { get; }

        private static string CreateNote(Game game, List<Trend> trend, Trend currentPrice)
        {
            var note = string.Empty;
            var minPrice = trend.Except(new[] { currentPrice }).Min(t => t.Price);

            if (minPrice == currentPrice.Price)
            {
                note = "Matches previous low";
            }
            else if (currentPrice.Price < minPrice)
            {
                note = "Lowest price ever";
            }
            //else if (currentPrice.Price > game.RegularPrice)
            //{
            //    note = "Price is higher than usual";
            //}

            return note;
        }
    }
}