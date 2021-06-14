using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;
using Red.Infrastructure.NintendoApi.Models;

namespace Red.Infrastructure.NintendoApi
{
    internal sealed class EshopConverter
    {
        private readonly ISlugBuilder _slugBuilder;
        private IAppLogger<EshopConverter> Log { get; }

        public EshopConverter(IAppLogger<EshopConverter> log, ISlugBuilder slugBuilder)
        {
            Log = log;
            _slugBuilder = slugBuilder;
        }

        public SwitchGameSale ConvertToGameSale(SalesSearchItem item)
        {
            var contentRating = BuildContentRating(item);

            return new()
            {
                Screenshots = item.Screenshots.Select(x => new ImageDetail{Url = x}).ToList(),
                Colors = item.DominantColors.Where(HexColor.IsValidCode).Select(x => new HexColor(x)).ToList(),
                HeroBannerUrl = item.HeroBannerUrl,
                IsNew = item.IsNew ?? false,
                ContentRating = contentRating,
                Title = item.FormalName ?? "",
                Nsuid = item.Nsuid ?? ""
            };
        }


        public SwitchGame ConvertToSwitchGame(LibrarySearchGame game)
        {
            // todo: use actual country
            var contentRating = BuildContentRating("DE", game);

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
                ContentRating = contentRating,
                EshopUrl = game.Url,
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
                Slug = _slugBuilder.Build(game.Title),
                // todo: Insert actual region
                Region = "EU",
                ProductCode = game.ProductCodeSS![0].Trim(),
                // todo: add missing fields
                Media = new SwitchGameMedia
                {
                    Cover = new ImageDetail { Url = game.image_url_sq_s ?? "" }
                },
            };
        }

        private static ContentRating BuildContentRating(SalesSearchItem item)
        {
            var contentDescriptors = item.RatingInfo?.ContentDescriptors
                                         .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                                         .Select(x => x.Name!)
                                         .ToList()
                                     ?? new List<string>();

            ContentRating contentRating = new();

            if (!string.IsNullOrWhiteSpace(item.RatingInfo?.RatingSystem?.Name)
                && item.RatingInfo?.Rating?.Age.HasValue == true)
            {
                contentRating = new()
                {
                    ContentDescriptors = contentDescriptors,
                    System = item.RatingInfo.RatingSystem.Name,
                    Age = item.RatingInfo.Rating.Age!.Value,
                    Provisional = item.RatingInfo?.Rating?.Provisional ?? false
                };
            }

            return contentRating;
        }

        private static CountryDictionary<ContentRating> BuildContentRating(string country, LibrarySearchGame game)
        {
            var contentRating = new CountryDictionary<ContentRating>();

            if (game.AgeRating.HasValue && !string.IsNullOrWhiteSpace(game.AgeRatingType))
            {
                contentRating[country] = new()
                {
                    System = game.AgeRatingType,
                    Age = (int) game.AgeRating
                };
            }

            return contentRating;
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

            if (string.Equals(text, "preorder", StringComparison.InvariantCultureIgnoreCase))
            {
                return EshopSalesStatus.PreOrder;
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