using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Red.Core.Application.Extensions;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider
{
    internal sealed class SwitchGameMerger : ISwitchGameMerger
    {
        private IAppLogger<SwitchGameMerger> Log { get; }

        public SwitchGameMerger(IAppLogger<SwitchGameMerger> log)
        {
            Log = log;
        }

        public SwitchGame MergeLibrary(SwitchGame t, SwitchGame s)
        {
            // CHECK ABNORMALITIES
            if (!t.Price.Equals(s.Price) && !Equals(s.Price, new()) && s.Price.Keys.Count > 0)
            {
                Log.LogCritical("Price actually should not be different when merging the library.. {targetEntity} {sourceEntity}", t, s);
                Debugger.Break();
                return t;
            }

            if (!string.Equals(t.FsId, s.FsId, StringComparison.InvariantCulture))
            {
                Log.LogCritical("FsId differs - please check this manually! {targetEntity} {sourceEntity}", t, s);
                Debugger.Break();
                return t;
            }

            var fsid = t.FsId;
            if (!string.Equals(t.FsId, s.FsId, StringComparison.InvariantCulture))
            {
                fsid = s.FsId;
            }

            // MERGE CATEGORIES
            IEnumerable<string> categories = t.Categories;
            if (!t.Categories.SequenceEqual(s.Categories))
            {
                categories = t.Categories
                              .Union(s.Categories)
                              .DistinctBy(x => x.ToLowerInvariant());
            }

            // MERGE LANGUAGES
            IEnumerable<string> languages = t.Languages;
            if (!t.Languages.SequenceEqual(s.Languages))
            {
                languages = t.Languages
                             .Union(s.Languages)
                             .DistinctBy(x => x.ToLowerInvariant());
            }

            // MERGE NSUIDS
            IEnumerable<string> nsuids = t.Nsuids;
            if (!t.Nsuids.SequenceEqual(s.Nsuids))
            {
                // nintendo might have decided to delete a nsuid because it was wrong - so don't merge but overwrite it
                nsuids = s.Nsuids;
            }

            // MERGE COLORS
            IEnumerable<HexColor> colors = t.Colors;
            if (!t.Colors.SequenceEqual(s.Colors))
            {
                var totalCount = t.Colors.Count + s.Colors.Count;
                var countSame = totalCount - t.Colors.Union(s.Colors).Distinct().Count();

                if (countSame >= 2)
                {
                    colors = t.Colors.Union(s.Colors).DistinctBy(x => x.HexCode.ToLowerInvariant());
                }
                else
                {
                    colors = s.Colors;
                }
            }

            // MERGE PLAYMODES
            var playModes = t.PlayModes;
            if (!t.PlayModes.Equals(s.PlayModes))
            {
                playModes = s.PlayModes;
            }

            // MERGE DESCRIPTION
            var description = t.Description.Merge(s.Description);

            // MERGE ESHOPURL
            var eshopUrl = t.EshopUrl.Merge(s.EshopUrl);

            // MERGE POPULARITY
            var popularity = t.Popularity;
            if (t.Popularity != s.Popularity)
            {
                popularity = s.Popularity;
            }

            // MERGE TITLE & SLUG
            var title = t.Title.Merge(s.Title);
            var slug = t.Title.Merge(s.Slug);

            var publisher = PickValue(x => x.Publisher, t, s);
            var developer = PickValue(x => x.Developer, t, s);
            var releaseDate = PickValue(x => x.ReleaseDate, t, s);
            var downloadSize = PickValue(x => x.DownloadSize, t, s);
            var minPlayers = PickValue(x => x.MinPlayers, t, s);
            var maxPlayers = PickValue(x => x.MaxPlayers, t, s);
            var coop = PickValue(x => x.Coop, t, s);
            var demoAvailable = PickValue(x => x.DemoAvailable, t, s);
            var supportsCloudSave = PickValue(x => x.SupportsCloudSave, t, s);
            var removedFromEshop = PickValue(x => x.RemovedFromEshop, t, s);
            var voucherPossible = PickValue(x => x.VoucherPossible, t, s);
            var contentRating = t.ContentRating.Merge(s.ContentRating);

            var result = t with
            {
                FsId = fsid,
                ContentRating = contentRating,
                Categories = categories.ToList(),
                Languages = languages.ToList(),
                Nsuids = nsuids.ToList(),
                Colors = colors.ToList(),
                PlayModes = playModes with { },
                Description = description,
                Popularity = popularity,
                Title = title,
                Slug = slug,
                EshopUrl = eshopUrl,
                Publisher = publisher,
                Developer = developer,
                ReleaseDate = releaseDate,
                DownloadSize = downloadSize,
                MinPlayers = minPlayers,
                MaxPlayers = maxPlayers,
                Coop = coop,
                DemoAvailable = demoAvailable,
                SupportsCloudSave = supportsCloudSave,
                RemovedFromEshop = removedFromEshop,
                VoucherPossible = voucherPossible
            };

            var diffBuilder = new ObjectDiffBuilder();
            var diff = diffBuilder.BuildText(t, result);

            if (diff != null)
            {
                Log.LogDebug(diff);
            }

            return result;
        }

        public SwitchGame MergePrice(CultureInfo culture, SwitchGame game, SwitchGamePrice price)
        {
            var updatedEntity = new PriceMerger(culture, game, price).MergePrice();

            if (!game.Price.Equals(updatedEntity.Price))
            {
                return updatedEntity;
            }

            return game;
        }

        private string? PickValue(Func<SwitchGame, string?> selector, SwitchGame a, SwitchGame b)
        {
            var valueA = selector(a);
            var valueB = selector(b);
            var result = valueA;

            if (!string.Equals(valueA, valueB, StringComparison.InvariantCulture)
                && !string.IsNullOrWhiteSpace(valueB))
            {
                result = valueB;
            }

            return result;
        }

        private T? PickValue<T>(Func<SwitchGame, T?> selector, SwitchGame a, SwitchGame b) where T : struct
        {
            var valueA = selector(a);
            var valueB = selector(b);
            var result = valueA;

            if (!Nullable.Equals(valueA, valueB) && valueB.HasValue)
            {
                result = valueB;
            }

            return result;
        }
    }
}