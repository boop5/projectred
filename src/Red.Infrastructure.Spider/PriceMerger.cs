using System;
using System.Collections.Generic;
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
            _lastPrice = game.Price.History[_country]?.OrderBy(x => x.Date).LastOrDefault();
        }

        private SwitchGamePriceDetails InitializeHistory(SwitchGamePriceDetails details)
        {
            if (!_price.Discounted && !string.IsNullOrWhiteSpace(_price.Currency))
            {
                if (_lastPrice == null || _price.RegularPrice.HasValue
                    && _lastPrice.Amount - _price.RegularPrice < 0.01)
                {
                    var history = details.History with { };
                    var localHistory = history[_country] ?? new List<DatedPrice>();
                    localHistory.Add(DatedPrice.New(_price.RegularPrice!.Value, _price.Currency!));
                    history[_country] = localHistory.Distinct().ToList();

                    return details with {History = history};
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
            var localAth = ath[_country];
            var highestPrice = Math.Max(_price.CurrentPrice ?? int.MinValue, _price.RegularPrice.Value);

            if (localAth == null || Math.Abs(localAth.Amount - highestPrice) < 0.01)
            {
                ath[_country] = Price.New(highestPrice, _price.Currency);

                return details with {AllTimeHigh = ath};
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
            var localAtl = atl[_country];
            var lowestPrice = Math.Min(_price.CurrentPrice ?? int.MaxValue, _price.RegularPrice.Value);

            if (localAtl == null || Math.Abs(localAtl.Amount - lowestPrice) >= 0.01)
            {
                atl[_country] = Price.New(lowestPrice, _price.Currency);

                return details with {AllTimeHigh = atl};
            }

            return details;
        }

        private SwitchGamePriceDetails UpdateDiscount(SwitchGamePriceDetails details)
        {
            details = details with {OnDiscount = _price.Discounted};

            if (_price.Discounted && _price.CurrentPrice.HasValue && !string.IsNullOrWhiteSpace(_price.Currency))
            {
                if (_lastPrice == null || Math.Abs(_lastPrice.Amount - (float) _price.CurrentPrice) >= 0.01)
                {
                    var history = _game.Price.History with { };
                    history[_country] ??= new List<DatedPrice>();
                    history[_country]!.Add(DatedPrice.New(_price.CurrentPrice!.Value, _price.Currency));

                    return details with
                    {
                        History = history
                    };
                }
            }

            return details;
        }

        private SwitchGamePriceDetails UpdateRegularPrice(SwitchGamePriceDetails details)
        {
            if (_price.RegularPrice.HasValue && !string.IsNullOrWhiteSpace(_price.Currency))
            {
                var rp = _game.Price.RegularPrice with { };
                rp[_country] = Price.New(_price.RegularPrice.Value, _price.Currency);

                if (details.RegularPrice[_country] == null
                    || !details.RegularPrice[_country]!.Equals(rp[_country]))
                {
                    return details with {RegularPrice = rp};
                }
            }

            return details;
        }

        private SwitchGamePriceDetails UpdateSalesStatus(SwitchGamePriceDetails details)
        {
            if (_price.SalesStatus != details.SalesStatus)
            {
                return details with
                {
                    SalesStatus = _price.SalesStatus
                };
            }

            return details;
        }

        public SwitchGame MergePrice()
        {
            var updatedPrice = _game.Price with { };
            updatedPrice = UpdateRegularPrice(updatedPrice);
            updatedPrice = UpdateDiscount(updatedPrice);
            updatedPrice = InitializeHistory(updatedPrice);
            updatedPrice = UpdateSalesStatus(updatedPrice);
            updatedPrice = UpdateAllTimeLow(updatedPrice);
            updatedPrice = UpdateAllTimeHigh(updatedPrice);

            var updatedEntity = _game with {Price = updatedPrice};
            if (!_game.Price.Equals(updatedEntity.Price))
            {
                return updatedEntity;
            }

            return _game;
        }
    }
}