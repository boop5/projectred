using System;
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
using Red.Core.Application;
using Red.Core.Application.Interfaces;
using Red.Core.Domain.Models;
#pragma warning disable

namespace Red.Infrastructure.Spider.Worker
{
    internal sealed class GalleriesLoader
    {
        private IDocument _document;
        private ObjectInstance? _galleries;
        private List<GalleryItem>? _galleryItems;
        private JsScriptingService _js;

        private async Task<IDocument> GetDocument(string url)
        {
            var config = Configuration.Default.WithDefaultLoader().WithJs();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);
            await document.WaitForReadyAsync();

            while (document.ReadyState != DocumentReadyState.Complete)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
            }

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
            if (_galleryItems == null)
            {
                _galleryItems = new List<GalleryItem>();
            }
            else
            {
                return _galleryItems;
            }

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

        public IEnumerable<ImageDetail> GetScreenshots()
        {
            var galleryItems = GetGalleryItems(_document, _js, _galleries);
            return GetImageDetails(GetImagesFromGalleryItems(galleryItems));
        }

        public async Task<IEnumerable<VideoDetail>> GetVideos()
        {
            var galleryItems = GetGalleryItems(_document, _js, _galleries);
            return await LoadVideoDetails(GetVideosFromGalleryItems(galleryItems));
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

        public async Task<bool> Init(string url)
        {
            _document = await GetDocument(url);
            _js = _document.Context.GetService<JsScriptingService>();
            _galleries = GetGalleries(_js, _document);

            return _galleries != null;
        }

        private async Task<IReadOnlyCollection<VideoDetail>> LoadVideoDetails(IEnumerable<Video> videos)
        {
            var result = videos.Where(x => !string.IsNullOrWhiteSpace(x.PlaylistUrl))
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
            return result;
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

    internal sealed class ScreenshotSpider : TimedWorker
    {
        private readonly IServiceProvider _serviceProvider;

        public ScreenshotSpider(IAppLogger<ScreenshotSpider> log,
                                IServiceProvider serviceProvider)
            : base(log)
        {
            _serviceProvider = serviceProvider;
        }

        protected override TimeSpan GetInitialDelay()
        {
            return TimeSpan.FromMinutes(60);
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromHours(24);
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
                         // .Where(x => x.Media.LastUpdated < DateTime.Today)
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
                    var loader = new GalleriesLoader();
                    var initialized = await loader.Init(url);

                    if (!initialized) continue;
                    var entity = await repo.GetByProductCode(dto.ProductCode);

                    var updatedEntity = entity with
                    {
                        Media = new SwitchGameMedia
                        {
                            Screenshots = entity.Media.Screenshots.Union(loader.GetScreenshots()).Distinct().ToList(),
                            Videos = entity.Media.Videos.Union(await loader.GetVideos()).Distinct().ToList(),
                            Cover = entity.Media.Cover,
                            LastUpdated = DateTime.UtcNow
                        }
                    };

                    if (!entity.Media.Equals(updatedEntity.Media))
                    {
                        Log.LogInformation($"Update {entity.ProductCode} ({updatedEntity.Media.Screenshots.Count} screenshots, {updatedEntity.Media.Videos.Count} videos");
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

        private class ScreenshotDto
        {
            public string EshopUrl { get; init; } = "";
            public SwitchGameMedia Pictures { get; init; } = new();
            public string ProductCode { get; init; } = "";
            public string Region { get; init; } = "";
        }
    }
}