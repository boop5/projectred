using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Common;
using AngleSharp.Dom;
using AngleSharp.Io;
using AngleSharp.Scripting;
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
        private class LOL
        {
            public bool isVideo { get; init; }
            public string video_id { get; init; }
            public string video_embed_url { get; init; }
            public string video_thumbnail_url{ get; init; }
            public string video_content_url { get; init; }
            public string filename { get; init; }
            public string image_url { get; init; }
            public string title { get; init; }
            public string descr { get; init; }
            public string type { get; init; }
        }

        private class Video
        {
            public string Playlist => $"https://production-ps.lvp.llnw.net/r/PlaylistService/media/{video_id}/getMobilePlaylistByMediaId";
            public string filename { get; init; }
            public string video_id { get; init; }
            public string video_embed_url { get; init; }
            public string video_thumbnail_url{ get; init; }
            public string video_content_url { get; init; }
        }

        private class Image
        {
            public string filename { get; init; } = "";
            public string image_url { get; init; } = "";
            public string title { get; init; } = "";
            public string descr { get; init; } = "";
        }

        private class mobileUrlItem
        {
            public string targetMediaPlatform { get; init; } = "";
            public string mobileUrl { get; init; } = "";
        }

        private class MediaListItem
        {
            public string mediaId { get; init; } = "";
            public int positionInChannel { get; init; } = 0;
            public int durationInMilliseconds { get; init; } = 0;
            public string title { get; init; } = "";
            public string description { get; init; } = "";
            public string thumbnailImageUrl { get; init; } = "";
            public string previewImageUrl { get; init; } = "";
            public List<string> flags { get; init; } = new();
            public List<mobileUrlItem> mobileUrls { get; init; } = new();

        }

        private class Playlist
        {
            public string orgId { get; init; } = "";
            public List<MediaListItem> mediaList { get; init; } = new();
        }

        private class ScreenshotDto
        {
            public string EshopUrl { get; set; } = "";
            public string ProductCode { get; set; } = "";
            public string Region { get; set; } = "";
            public SwitchGameMedia Pictures { get; set; } = new();
        }

        private readonly IServiceProvider _serviceProvider;

        public ScreenshotSpider(ILogger<ScreenshotSpider> log,
                                IServiceProvider serviceProvider)
            : base(log)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            ISwitchGameRepository repo = _serviceProvider.GetRequiredService<ISwitchGameRepository>();

            var screenshotDtos = await repo.Get()
                                     .Where(x => !string.IsNullOrWhiteSpace(x.EshopUrl))
                                     .OrderBy(x => x.ProductCode)
                                     .Take(50)
                                     .Select(x => new ScreenshotDto
                                     {
                                         EshopUrl = x.EshopUrl!,
                                         Pictures = x.Media,
                                         ProductCode = x.ProductCode,
                                         Region = x.Region
                                     }).ToListAsync(stoppingToken);

            foreach (var dto in screenshotDtos)
            {
                var url = $"https://www.nintendo-europe.com/{dto.EshopUrl}#Gallery";
                var config = Configuration.Default.WithDefaultLoader().WithJs();
                var context = BrowsingContext.New(config);
                var js = context.GetService<JsScriptingService>();
                var document = await context.OpenAsync(url);
                await document.WaitForReadyAsync();

                Jint.Native.Object.ObjectInstance galleries = (Jint.Native.Object.ObjectInstance) js.EvaluateScript(document, "galleries");
                var objects = new List<LOL>();

                foreach (var (key, _) in galleries.GetOwnProperties())
                {
                    var array = (Jint.Native.Array.ArrayInstance) js.EvaluateScript(document, $"galleries[{key}]");
                    var length = array.GetProperty("length").Value.AsNumber();
                    for (var i = 0; i < length; i++)
                    {
                        var item = (Jint.Native.Object.ObjectInstance)js.EvaluateScript(document, $"galleries[{key}][{i}]");

                        objects.Add(new LOL()
                        {
                            isVideo = item.GetProperty("isVideo").Value.AsBoolean(),
                            video_id = item.GetProperty("video_id").Value.AsString(),
                            video_embed_url = item.GetProperty("video_embed_url").Value.AsString(),
                            video_thumbnail_url = item.GetProperty("video_thumbnail_url").Value.AsString(),
                            video_content_url = item.GetProperty("video_content_url").Value.AsString(),
                            filename = item.GetProperty("filename").Value.AsString(),
                            image_url = item.GetProperty("image_url").Value.AsString(),
                            title = item.GetProperty("title").Value.AsString(),
                            descr = item.GetProperty("descr").Value.AsString(),
                            type = item.GetProperty("type").Value.AsString(),
                        });
                    }
                }

                var images = objects.Where(x => !x.isVideo)
                                    .Select(x => new Image()
                                    {
                                        filename = x.filename,
                                        descr = x.descr,
                                        image_url = x.image_url,
                                        title = x.title
                                    })
                                    .ToList();


                var videos = objects.Where(x => x.isVideo)
                                    .Select(
                                        x => new Video()
                                        {
                                            filename = x.filename,
                                            video_content_url = x.video_content_url,
                                            video_embed_url = x.video_embed_url,
                                            video_id = x.video_id,
                                            video_thumbnail_url = x.video_thumbnail_url
                                        }).ToList();

                foreach (var video in videos.Where(x => !string.IsNullOrWhiteSpace(x.Playlist)))
                {
                    var response = await new HttpClient().GetAsync(video.Playlist, stoppingToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync(stoppingToken);
                        var playlist = JsonSerializer.Deserialize<Playlist>(body);
                        ;
                    }
                }
            }
        }

        protected override TimeSpan GetInitialDelay()
        {
            return TimeSpan.FromMinutes(0);
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }
    }
}
