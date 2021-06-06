using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Red.Core.Application;
using Red.Infrastructure.Spider.Nintendo;
using Red.Infrastructure.Spider.Util;

namespace Red.Infrastructure.Spider.Worker
{
    public class LibrarySpider : ScheduledWorker
    {
        public LibrarySpider(ILogger<LibrarySpider> log) 
            :base(log)
        {
        }

        protected override async Task LoopAsync(CancellationToken stoppingToken = default)
        {
            // await DoLibrary();
            await DoPrice();

        }

        private static async Task DoPrice()
        {
            // 70010000020399 70010000003781 70010000001020
            var hch = new HttpClientHandler {Proxy = null, UseProxy = false};
            var http = new HttpClient(hch) {Timeout = TimeSpan.FromSeconds(120)};
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var url =
                "https://api.ec.nintendo.com/v1/price?country=DE&ids=70010000003781&ids=70010000001020&ids=70010000010138&ids=70010000020399&lang=en&limit=50";

            var response = await http.GetAsync(url).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var deserialized = JsonSerializer.Deserialize<PriceSearchResult>(body);
                ;
            }
        }
        private static async Task DoLibrary()
        {
            var hch = new HttpClientHandler {Proxy = null, UseProxy = false};
            var http = new HttpClient(hch) {Timeout = TimeSpan.FromSeconds(120)};
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var language = "en";
            var term = "*";
            var baseUrl = $"{Constants.NintendoEUUrl}/{language}";
            var filter = $"q={term}&start={0}&rows={100}" +
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
                ;
            }
        }

        protected override TimeSpan GetTaskInterval()
        {
            return TimeSpan.FromMinutes(5);
        }
    }
}
