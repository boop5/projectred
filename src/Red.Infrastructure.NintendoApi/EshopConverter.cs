using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;
using Red.Infrastructure.NintendoApi.Models;

namespace Red.Infrastructure.NintendoApi
{
    internal sealed class EshopConverter
    {
        private readonly ISlugBuilder _slugBuilder;
        private ILogger<EshopConverter> Log { get; }

        public EshopConverter(ILogger<EshopConverter> log, ISlugBuilder slugBuilder)
        {
            Log = log;
            _slugBuilder = slugBuilder;
        }

        private string? BuildSlug(LibrarySearchGame game)
        {
            if (string.IsNullOrWhiteSpace(game.Title))
            {
                return null;
            }

            try
            {
                return _slugBuilder.Build(game.Title);
            }
            catch (Exception e)
            {
                Log.LogWarning(e, "Failed to build slug for {game}", game);
                return null;
            }
        }

        public SwitchGame ConvertToSwitchGame(LibrarySearchGame game)
        {
            return new()
            {
                Nsuids = game.Nsuids ?? new List<string>(),
                Languages = game.Languages ?? new List<string>(),
                Categories = game.GameCategoriesPretty ?? new List<string>(),
                PlayModes = new SwitchGamePlayModes()
                {
                    Handheld = game.HandheldMode == true,
                    Tabletop = game.TabletopMode == true,
                    Tv = game.TvMode == true
                },
                EshopUrl = game.Url,
                AgeRating = game.AgeRating,
                Coop = game.CoopPlay,
                DemoAvailable = game.DemoAvailable,
                Developer = game.Developer,
                Publisher = game.Publisher,
                VoucherPossible = game.SwitchGameVoucher,
                Description = game.Excerpt,
                MinPlayers = game.MinPlayers,
                MaxPlayers = game.MaxPlayers,
                Popularity = game.Popularity ?? 0,
                ReleaseDate = game.ReleaseDate,
                RemovedFromEshop = game.RemovedFromEshop,
                Title = game.Title,
                SupportsCloudSave = game.SupportsCloudSave,
                Slug = BuildSlug(game),
                // todo: Insert actual region
                Region = "EU",
                ProductCode = game.ProductCodeSS![0].Trim(),
                // todo: add missing fields
                Pictures = new SwitchGamePictures
                {
                    Cover = game.image_url_sq_s
                },
            };
        }


        public SwitchGamePrice ConvertToSwitchGamePrice(PriceSearchItem price)
        {
            float? regularPrice = null;
            float? currentPrice;

            if (float.TryParse(price.RegularPrice.RawValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var rp))
            {
                regularPrice = rp;
            }

            currentPrice = float.TryParse(
                price.DiscountPrice?.RawValue,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out var dp)
                ? dp
                : regularPrice;

            // todo: remove .ToString()
            return new SwitchGamePrice(price.Nsuid!.ToString())
            {
                SalesStatus = StringToSalesStatus(price.SalesStatus),
                RegularPrice = regularPrice,
                CurrentPrice = currentPrice,
                Currency = price.RegularPrice.Currency,
                Discounted = currentPrice < regularPrice
            };
        }

        private EshopSalesStatus StringToSalesStatus(string? text)
        {
            if (string.Equals(text, "onsale", StringComparison.InvariantCultureIgnoreCase))
            {
                return EshopSalesStatus.OnSale;
            }

            if (string.Equals(text, "sales_termination", StringComparison.InvariantCultureIgnoreCase))
            {
                return EshopSalesStatus.SalesTermination;
            }

            if (string.Equals(text, "not_found", StringComparison.InvariantCultureIgnoreCase))
            {
                return EshopSalesStatus.NotFound;
            }

            if (string.Equals(text, "unreleased", StringComparison.InvariantCultureIgnoreCase))
            {
                return EshopSalesStatus.Unreleased;
            }

            if (text != null)
            {
                Log.LogWarning($"Found unknown SalesStatus '{text}'");
                Debugger.Break();
            }

            return EshopSalesStatus.Unknown;
        }
    }
}