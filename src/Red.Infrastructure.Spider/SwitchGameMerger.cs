using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Red.Core.Application.Extensions;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider
{
    public sealed class SwitchGameMerger : ISwitchGameMerger
    {
        private IAppLogger<SwitchGameMerger> Log { get; }

        public SwitchGameMerger(IAppLogger<SwitchGameMerger> log)
        {
            Log = log;
        }

        public SwitchGame Merge(SwitchGame t, SwitchGame s)
        {
            // CHECK ABNORMALITIES
            if (!string.Equals(t.ProductCode, s.ProductCode, StringComparison.InvariantCultureIgnoreCase))
            {
                Debugger.Break();

                if (string.Equals(t.Title, s.Title, StringComparison.InvariantCultureIgnoreCase))
                {
                    Log.LogCritical("ProductCode differs but the titles are equal. "
                                    + "Might be a duplicated name (ie Minefield) "
                                    + "- please check this manually! {targetEntity} {sourceEntity}", t, s);
                    return t;
                }
                else
                {
                    Log.LogCritical("ProductCode differs - please check this manually! {targetEntity} {sourceEntity}", t, s);
                }
            }

            if (!string.Equals(t.Region, s.Region, StringComparison.InvariantCultureIgnoreCase))
            {
                Log.LogCritical("You are trying to merge games from different regions! {targetEntity} {sourceEntity}", t, s);
                Debugger.Break();
                return t;
            }

            if (!t.Price.Equals(s.Price))
            {
                Log.LogCritical("Price actually should not be different when merging the library.. {targetEntity} {sourceEntity}", t, s);
                Debugger.Break();
                return t;
            }

            if (!t.Media.Equals(s.Media))
            {
                Log.LogCritical("Media actually should not be different when merging the library.. {targetEntity} {sourceEntity}", t, s);
                Debugger.Break();
                return t;
            }

            if (!string.IsNullOrWhiteSpace(t.FsId)
                && !string.IsNullOrWhiteSpace(s.FsId)
                && !string.Equals(t.FsId, s.FsId, StringComparison.InvariantCulture))
            {
                Log.LogCritical("FsId differs - please check this manually! {targetEntity} {sourceEntity}", t, s);
                Debugger.Break();
                return t;
            }

            var fsid = t.FsId;
            if (string.IsNullOrWhiteSpace(t.FsId) && !string.IsNullOrWhiteSpace(s.FsId))
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

            // MERGE POPULARITY
            var popularity = t.Popularity;
            if (t.Popularity != s.Popularity)
            {
                popularity = s.Popularity;
            }

            // MERGE TITLE & SLUG
            var title = t.Title;
            var slug = t.Slug;
            if (!string.Equals(t.Title, s.Title, StringComparison.InvariantCulture)
                && !string.IsNullOrWhiteSpace(s.Title))
            {
                title = s.Title;
                slug = s.Slug;
            }

            var eshopUrl = PickValue(x => x.EshopUrl, t, s);
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

            LogUpdates(t, result);

            return result;
        }


        private string? BuildUpdateString(object? o)
        {
            if (o is IEnumerable<object> enumerable)
            {
                var values = string.Join(",", enumerable.Select(x => BuildUpdateString(x)));
                return $"[{values}]";
            }

            return o?.ToString() ?? null;
        }
        private string BuildUpdateString(object? a, object? b)
        {
            var stringA = BuildUpdateString(a);
            var stringB = BuildUpdateString(b);

            if(string.IsNullOrWhiteSpace(stringA)) stringA= "NULL";
            if(string.IsNullOrWhiteSpace(stringB)) stringB= "NULL";

            return $"'{stringA}' -> '{stringB}'";
        }

        private void LogUpdates(object targetEntity, object mergedEntity)
        {
            if (targetEntity.GetType() != mergedEntity.GetType())
            {
                // todo: LogWarning
                return;
            }

            if (Equals(targetEntity, mergedEntity))
            {
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Merged two entities '{targetEntity.GetType().Name}'");

            var hasUpdate = false;
            foreach (var propertyInfo in targetEntity.GetType().GetProperties().Where(x => x.CanRead))
            {
                var targetValue = propertyInfo.GetValue(targetEntity);
                var mergedValue = propertyInfo.GetValue(mergedEntity);

                if (targetValue is IEnumerable<object> enumerableA && mergedValue is IEnumerable<object> enumerableB)
                {
                    if (!enumerableA.SequenceEqual(enumerableB))
                    {
                        hasUpdate = true;
                        sb.AppendLine($"\t\t{propertyInfo.Name}: {BuildUpdateString(enumerableA, enumerableB)}");
                    }
                }
                else if(!Equals(targetValue, mergedValue))
                {
                    hasUpdate = true;
                    sb.AppendLine($"\t\t{propertyInfo.Name}: {BuildUpdateString(targetValue, mergedValue)}");
                }
            }

            if (hasUpdate)
            {
                Log.LogDebug(sb.ToString());
            }
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