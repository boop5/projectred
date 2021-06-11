﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Scripting;
using Jint.Native.Array;
using Jint.Native.Object;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Red.Core.Application;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;

namespace Red.Infrastructure.Spider.Worker
{
    internal sealed class ScreenshotSpider : ScheduledWorker
    {
        private readonly IServiceProvider _serviceProvider;

        public ScreenshotSpider(ILogger<ScreenshotSpider> log,
                                IServiceProvider serviceProvider)
            : base(log)
        {
            _serviceProvider = serviceProvider;
        }

        private async Task<IDocument> GetDocument(string url)
        {
            var config = Configuration.Default.WithDefaultLoader().WithJs();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);
            await document.WaitForReadyAsync();

            return document;
        }

        private ObjectInstance? GetGalleries(JsScriptingService js, IDocument document)
        {
            if (document.ReadyState != DocumentReadyState.Complete)
            {
                return null;
            }

            try
            {
                if (js.EvaluateScript(document, "typeof galleries") as string == "undefined")
                {
                    return null;
                }

                return js.EvaluateScript(document, "galleries") as ObjectInstance;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private IReadOnlyCollection<GalleryItem> GetGalleryItems(IDocument document, JsScriptingService js, ObjectInstance galleries)
        {
            var objects = new List<GalleryItem>();

            foreach (var (key, _) in galleries.GetOwnProperties())
            {
                if (js.EvaluateScript(document, $"galleries[{key}]") is ArrayInstance array)
                {
                    var length = array.GetProperty("length").Value.AsNumber();
                    for (var i = 0; i < length; i++)
                    {
                        if (js.EvaluateScript(document, $"galleries[{key}][{i}]") is ObjectInstance item)
                        {
                            string GetValue(string k)
                            {
                                return item.HasOwnProperty(k)
                                    ? item.GetOwnProperty(k).Value.AsString()
                                    : string.Empty;
                            }

                            objects.Add(
                                new GalleryItem
                                {
                                    isVideo = item.HasOwnProperty("isVideo") && item.GetOwnProperty("isVideo").Value.AsBoolean(),
                                    video_id = GetValue("video_id"),
                                    video_embed_url = GetValue("video_embed_url"),
                                    video_thumbnail_url = GetValue("video_thumbnail_url"),
                                    video_content_url = GetValue("video_content_url"),
                                    filename = GetValue("filename"),
                                    image_url = GetValue("image_url"),
                                    title = GetValue("title"),
                                    descr = GetValue("descr"),
                                    type = GetValue("type")
                                });
                        }
                    }
                }
            }

            return objects;
        }

        private IReadOnlyCollection<ImageDetail> GetImageDetails(IReadOnlyCollection<Image> images)
        {
            return images.Select(
                             x => new ImageDetail
                             {
                                 Url = x.image_url,
                                 Title = x.title
                             })
                         .ToList();
        }

        private IReadOnlyCollection<Image> GetImagesFromGalleryItems(IEnumerable<GalleryItem> items)
        {
            return items.Where(x => !x.isVideo)
                        .Select(
                            x => new Image
                            {
                                filename = x.filename,
                                descr = x.descr,
                                image_url = x.image_url,
                                title = x.title
                            })
                        .ToList();
        }

        protected override TimeSpan GetInitialDelay()
        {
            return TimeSpan.FromMinutes(0);
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }

        private IReadOnlyCollection<Video> GetVideosFromGalleryItems(IEnumerable<GalleryItem> items)
        {
            return items.Where(x => x.isVideo)
                        .ToList()
                        .Select(
                            x => new Video
                            {
                                filename = x.filename,
                                video_content_url = x.video_content_url,
                                video_embed_url = x.video_embed_url,
                                video_id = x.video_id,
                                video_thumbnail_url = x.video_thumbnail_url
                            })
                        .ToList();
        }

        private async Task<IReadOnlyCollection<VideoDetail>> LoadVideoDetails(IEnumerable<Video> videos)
        {
            //var result = new List<VideoDetail>();

            var result2 = videos.Where(x => !string.IsNullOrWhiteSpace(x.PlaylistUrl))
                                .Select(async x => await new HttpClient().GetAsync(x.PlaylistUrl))
                                .Select(x => x.Result)
                                .Where(x => x.IsSuccessStatusCode)
                                .Select(async x => await x.Content.ReadAsStringAsync())
                                .Select(x => x.Result)
                                .Select(x => JsonSerializer.Deserialize<Playlist>(x))
                                .Where(x => x != null)
                                .SelectMany(x => x.mediaList)
                                .Where(x => x.mobileUrls.Any(x => x.targetMediaPlatform == "MobileH264"))
                                .Select(
                                    x =>
                                        new VideoDetail
                                        {
                                            Title = x.title,
                                            Url = x.mobileUrls.First(y => y.targetMediaPlatform == "MobileH264").mobileUrl,
                                            Duration = x.durationInMilliseconds,
                                            PreviewImage = x.previewImageUrl
                                        })
                                .ToList();

            /*
            foreach (var video in videos.Where(x => !string.IsNullOrWhiteSpace(x.PlaylistUrl)))
            {
                var response = await new HttpClient().GetAsync(video.PlaylistUrl);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var playlist = JsonSerializer.Deserialize<Playlist>(body);

                    if (playlist is not null)
                    {
                        foreach (var item in playlist.mediaList)
                        {
                            var h264Video = item.mobileUrls.FirstOrDefault(x => x.targetMediaPlatform == "MobileH264");

                            if (h264Video is not null && !string.IsNullOrWhiteSpace(h264Video.mobileUrl))
                            {
                                result.Add(
                                    new VideoDetail
                                    {
                                        Title = item.title,
                                        Url = h264Video.mobileUrl,
                                        Duration = item.durationInMilliseconds,
                                        PreviewImage = item.previewImageUrl
                                    });
                            }
                        }
                    }
                }
            }

            var same = result.SequenceEqual(result2);
            */

            return result2;
            //return result;
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            ISwitchGameRepository repo = _serviceProvider.GetRequiredService<ISwitchGameRepository>();
            var gs = await repo.Get()
                               .Select(
                                   x => new SwitchGame
                                   {
                                       ProductCode = x.ProductCode,
                                       Region = x.Region,
                                       Media = x.Media,
                                       EshopUrl = x.EshopUrl
                                   })
                               .ToListAsync();
            var dtos = gs.Where(x => !string.IsNullOrWhiteSpace(x.EshopUrl))
                         .Where(x => x.Media.LastUpdated < DateTime.Today)
                         .OrderBy(x => x.ProductCode)
                         .Select(
                             x => new ScreenshotDto
                             {
                                 EshopUrl = x.EshopUrl!,
                                 Pictures = x.Media,
                                 ProductCode = x.ProductCode,
                                 Region = x.Region
                             })
                         .ToList();

            // await Task.WhenAll(dtos.ChunkBy(500).Select(UpdateScreenshots).ToList());
            Log.LogWarning($"UPDATE {dtos.Count} games");
            await UpdateScreenshots(dtos);
        }

        private async Task UpdateScreenshots(IEnumerable<ScreenshotDto> chunk)
        {
            ISwitchGameRepository repo = _serviceProvider.GetRequiredService<ISwitchGameRepository>();

            foreach (var dto in chunk)
            {
                var start = DateTime.UtcNow;
                Log.LogInformation(dto.ProductCode);

                try
                {
                    var url = $"https://www.nintendo-europe.com/{dto.EshopUrl}#Gallery";
                    var document = await GetDocument(url);
                    var js = document.Context.GetService<JsScriptingService>();

                    var galleries = GetGalleries(js, document);
                    if (galleries == null)
                    {
                        continue;
                    }

                    var galleryItems = GetGalleryItems(document, js, galleries);
                    var entity = await repo.GetByProductCode(dto.ProductCode);
                    var screenshots = entity.Media.Screenshots
                                                                         .Union(GetImageDetails(GetImagesFromGalleryItems(galleryItems)))
                                                                         .Distinct()
                                                                         .ToList();
                    var videos = entity.Media.Videos
                                                                    .Union(await LoadVideoDetails(GetVideosFromGalleryItems(galleryItems)))
                                                                    .Distinct()
                                                                    .ToList();
                    var updatedEntity = entity with
                    {
                        Media = new SwitchGameMedia
                        {
                            Screenshots = screenshots,
                            Videos = videos,
                            Cover = entity.Media.Cover,
                            LastUpdated = DateTime.UtcNow
                        }
                    };

                    if (!entity.Media.Equals(updatedEntity.Media))
                    {
                        Log.LogInformation($"Update {entity.ProductCode}");
                        await repo.UpdateAsync(updatedEntity);
                    }

                    Log.LogInformation($"Finished after {(DateTime.UtcNow - start).TotalSeconds}s");
                }
                catch (Exception e)
                {
                    Log.LogError("FAILED SCREENSHOT THING {msg}", e.Message);
                }
            }
        }


        #region DTO

        // ReSharper disable InconsistentNaming

        private class Image
        {
            public string descr { get; init; } = "";
            public string filename { get; init; } = "";
            public string image_url { get; init; } = "";
            public string title { get; init; } = "";
        }

        [DebuggerDisplay("GalleryItem ({ isVideo ? \"Video\" : \"Image\" ,nq})")]
        private class GalleryItem
        {
            public string descr { get; init; } = "";
            public string filename { get; init; } = "";
            public string image_url { get; init; } = "";
            public bool isVideo { get; init; }
            public string title { get; init; } = "";
            public string type { get; init; } = "";
            public string video_content_url { get; init; } = "";
            public string video_embed_url { get; init; } = "";
            public string video_id { get; init; } = "";
            public string video_thumbnail_url { get; init; } = "";
        }

        private class MediaListItem
        {
            public string description { get; init; } = "";
            public int durationInMilliseconds { get; } = 0;
            public List<string> flags { get; init; } = new();
            public string mediaId { get; init; } = "";
            public List<mobileUrlItem> mobileUrls { get; } = new();
            public int positionInChannel { get; init; } = 0;
            public string previewImageUrl { get; } = "";
            public string thumbnailImageUrl { get; init; } = "";
            public string title { get; } = "";
        }

        private class mobileUrlItem
        {
            public string mobileUrl { get; } = "";
            public string targetMediaPlatform { get; } = "";
        }

        private class Playlist
        {
            public List<MediaListItem> mediaList { get; init; } = new();
            public string orgId { get; init; } = "";
        }

        private class ScreenshotDto
        {
            public string EshopUrl { get; init; } = "";
            public SwitchGameMedia Pictures { get; init; } = new();
            public string ProductCode { get; init; } = "";
            public string Region { get; init; } = "";
        }

        private class Video
        {
            public string filename { get; init; } = "";

            public string PlaylistUrl =>
                $"https://production-ps.lvp.llnw.net/r/PlaylistService/media/{video_id}/getMobilePlaylistByMediaId";

            public string video_content_url { get; init; } = "";
            public string video_embed_url { get; init; } = "";
            public string video_id { get; init; } = "";
            public string video_thumbnail_url { get; init; } = "";
        }

        // ReSharper restore InconsistentNaming

        #endregion
    }
}