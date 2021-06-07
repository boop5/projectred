using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Red.Core.Application;
using Red.Core.Application.Interfaces;
using Red.Infrastructure.Spider.Nintendo;
using Red.Infrastructure.Spider.Util;

namespace Red.Infrastructure.Spider.Worker
{
    public class LibrarySpider : ScheduledWorker
    {
        private readonly ISwitchGameRepository repo;

        public LibrarySpider(ILogger<LibrarySpider> log, ISwitchGameRepository repo) 
            :base(log)
        {
            this.repo = repo;
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            // 1. load from eshop api
            // 2. add new games
            // 3. update existing games


            // var games = await DoLibrary();

            






            //await repo.AddAsync(new SwitchGame
            //{
            //    Id = Guid.NewGuid(),
            //    Created = DateTime.UtcNow,
            //    Title = "Test",
            //    Slug = "boi",
            //    Categories = new List<string> { "foo", "bar", "baz" }
            //});
            //await repo.AddAsync(new SwitchGame
            //{
            //    Id = Guid.NewGuid(),
            //    Created = DateTime.UtcNow,
            //    Title = "Test 2",
            //    Slug = "boi-2",
            //});
            //await repo.AddAsync(new SwitchGame
            //{
            //    Id = Guid.NewGuid(),
            //    Created = DateTime.UtcNow,
            //    Title = "Test 3",
            //    Slug = "boi-3",
            //    Categories = new()
            //}); 
            //await repo.AddAsync(new SwitchGame
            //{
            //    Id = Guid.NewGuid(),
            //    Created = DateTime.UtcNow,
            //    Title = "Test 4",
            //    Slug = "boi-4",
            //    Categories = new(),
            //    PriceHistory = new List<PriceRecord>()
            //    {
            //        new PriceRecord{Date = DateTime.Now, Price = 123.45m}
            //    }
            //});

            var games = await repo.Get().ToListAsync();

            ;












            //await using var ctx = new LibraryContext();
            //await ctx.Games.AddAsync(new SwitchGame
            //{
            //    Id = Guid.NewGuid(),
            //    Created = DateTime.UtcNow,
            //    Title = "Test",
            //    Slug = "boi",
            //    Categories = new List<string> { "foo", "bar", "baz" }
            //});
            //await ctx.Games.AddAsync(new SwitchGame
            //{
            //    Id = Guid.NewGuid(),
            //    Created = DateTime.UtcNow,
            //    Title = "Test 2",
            //    Slug = "boi-2",
            //});
            //await ctx.Games.AddAsync(new SwitchGame
            //{
            //    Id = Guid.NewGuid(),
            //    Created = DateTime.UtcNow,
            //    Title = "Test 3",
            //    Slug = "boi-3",
            //    Categories = new()
            //}); 
            //await ctx.Games.AddAsync(new SwitchGame
            //{
            //    Id = Guid.NewGuid(),
            //    Created = DateTime.UtcNow,
            //    Title = "Test 4",
            //    Slug = "boi-4",
            //    Categories = new(),
            //    PriceHistory = new List<PriceRecord>()
            //    {
            //        new PriceRecord{Date = DateTime.Now, Price = 123.45m}
            //    }
            //});
            //await ctx.SaveChangesAsync();
            //var games = await ctx.Games.ToListAsync();

            ;





            // var games = await DoLibrary();
            // var ids = games.Where(x => x.Nsuid != null).Select(x => x.Nsuid!);
            // 
            // foreach (var chunk in ids.ChunkBy(50))
            // {
            //     await DoPrice(chunk!);
            // }
        }

        private static async Task DoPrice(IEnumerable<string> ids)
        {
            // 70010000020399 70010000003781 70010000001020
            var hch = new HttpClientHandler {Proxy = null, UseProxy = false};
            var http = new HttpClient(hch) {Timeout = TimeSpan.FromSeconds(120)};
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //var url = "https://api.ec.nintendo.com/v1/price?country=DE&ids=70010000003781&ids=70010000001020&ids=70010000010138&ids=70010000020399&lang=en&limit=50";
            var q = string.Join(null, ids.Take(50).Select(x => $"&ids={x}"));
            var url = $"https://api.ec.nintendo.com/v1/price?country=DE{q}&lang=en&limit=50";

            var response = await http.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var deserialized = JsonSerializer.Deserialize<PriceSearchResult>(body);
                ;
            }
        }
        private static async Task<IEnumerable<LibrarySearchGame>> DoLibrary()
        {
            var hch = new HttpClientHandler {Proxy = null, UseProxy = false};
            var http = new HttpClient(hch) {Timeout = TimeSpan.FromSeconds(120)};
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var language = "en";
            var term = "*";
            var baseUrl = $"{Constants.NintendoEUUrl}/{language}";
            var filter = $"q={term}&start={0}&rows={int.MaxValue}" +
                         "&fq=type:GAME " +
                         "AND ((playable_on_txt: \"HAC\")) " +
                         "AND system_type:nintendoswitch* " +
                         "AND sorting_title:* " +
                         "AND *:*";
            var url = $"{baseUrl}/select?{filter}";
            var response = await http.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var deserialized = JsonSerializer.Deserialize<LibrarySearchResult>(body);

                if (deserialized != null)
                {
                    return deserialized.Response.Games;
                }
            }

            return new List<LibrarySearchGame>();
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }
    }
}
