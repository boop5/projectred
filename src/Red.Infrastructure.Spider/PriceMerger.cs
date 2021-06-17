using System;
using System.Globalization;
using System.Linq;
using Red.Core.Application.Extensions;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider
{
    internal class PriceMerger
    {
        private readonly SwitchGame _game;
        private readonly SwitchGamePrice _price;
        private readonly string _country;
        private readonly DatedPrice? _lastPrice;

        public PriceMerger(CultureInfo culture, SwitchGame game, SwitchGamePrice price)
        {
            _game = game;
            _price = price;
            _country = culture.GetTwoLetterISORegionName();
            _lastPrice = game.Price[_country]?.History.OrderBy(x => x.Date).LastOrDefault();
        }

        private bool PriceDifferent(float? a, float? b)
        {
            if (a == null || b == null)
            {
                return true;
            }

            return !(Math.Abs(a.Value - b.Value) < 0.01);
        }

        private SwitchGamePriceDetails InitializeHistory(SwitchGamePriceDetails details)
        {
            if (!_price.Discounted && !string.IsNullOrWhiteSpace(_price.Currency))
            {
                if (_lastPrice == null || _price.RegularPrice.HasValue
                    && PriceDifferent(_lastPrice.Amount, _price.RegularPrice))
                {
                    var localHistory = details.History.ToList();
                    localHistory.Add(DatedPrice.New(_price.RegularPrice!.Value, _price.Currency!));

                    return details with { History = localHistory.Distinct().ToList() };
                }
            }

            return details;
        }

        private SwitchGamePriceDetails UpdateAllTimeHigh(SwitchGamePriceDetails details)
        {
            if (!_price.RegularPrice.HasValue || string.IsNullOrWhiteSpace(_price.Currency))
            {
                return details;
            }

            var ath = details.AllTimeHigh with { };
            var highestPrice = Math.Max(_price.CurrentPrice ?? int.MinValue, _price.RegularPrice.Value);

            if (ath == null || PriceDifferent(ath.Amount, highestPrice))
            {
                ath = Price.New(highestPrice, _price.Currency);

                return details with { AllTimeHigh = ath };
            }

            return details;
        }

        private SwitchGamePriceDetails UpdateAllTimeLow(SwitchGamePriceDetails details)
        {
            if (!_price.RegularPrice.HasValue || string.IsNullOrWhiteSpace(_price.Currency))
            {
                return details;
            }

            var atl = details.AllTimeLow with { };
            var lowestPrice = Math.Min(_price.CurrentPrice ?? int.MaxValue, _price.RegularPrice.Value);

            if (atl == null || PriceDifferent(atl.Amount, lowestPrice))
            {
                atl = Price.New(lowestPrice, _price.Currency);

                return details with { AllTimeLow = atl };
            }

            return details;
        }

        private SwitchGamePriceDetails UpdateDiscount(SwitchGamePriceDetails details)
        {
            details = details with { OnDiscount = _price.Discounted };

            if (_price.Discounted && _price.CurrentPrice.HasValue && !string.IsNullOrWhiteSpace(_price.Currency))
            {
                if (_lastPrice == null || PriceDifferent(_lastPrice.Amount, (float)_price.CurrentPrice))
                {
                    var history = details.History.ToList();
                    history.Add(DatedPrice.New(_price.CurrentPrice!.Value, _price.Currency));

                    return details with { History = history };
                }
            }

            return details;
        }

        private SwitchGamePriceDetails UpdateRegularPrice(SwitchGamePriceDetails details)
        {
            if (_price.RegularPrice.HasValue && !string.IsNullOrWhiteSpace(_price.Currency))
            {
                var rp = Price.New(_price.RegularPrice.Value, _price.Currency);

                if (details.RegularPrice == null || !details.RegularPrice!.Equals(rp))
                {
                    return details with { RegularPrice = rp };
                }
            }

            return details;
        }

        private SwitchGamePriceDetails UpdateSalesStatus(SwitchGamePriceDetails details)
        {
            if (_price.SalesStatus != details.SalesStatus)
            {
                return details with { SalesStatus = _price.SalesStatus };
            }

            return details;
        }

        public SwitchGame MergePrice()
        {
            var priceCopy = _game.Price.Merge(_game.Price); // clone
            if (priceCopy[_country] == null)
            {
                priceCopy[_country] = new SwitchGamePriceDetails();
            }

            var updatedDetails = priceCopy[_country]! with { };
            updatedDetails = UpdateRegularPrice(updatedDetails);
            updatedDetails = UpdateDiscount(updatedDetails);
            updatedDetails = InitializeHistory(updatedDetails);
            updatedDetails = UpdateSalesStatus(updatedDetails);
            updatedDetails = UpdateAllTimeLow(updatedDetails);
            updatedDetails = UpdateAllTimeHigh(updatedDetails);

            priceCopy[_country] = updatedDetails;

            var updatedEntity = _game with { Price = priceCopy };

            if (!_game.Price.Equals(updatedEntity.Price))
            {
                return updatedEntity;
            }

            return _game;
        }
    }
}