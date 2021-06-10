using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Red.Infrastructure.NintendoApi
{
    internal sealed class EshopHttpClient : HttpClient
    {
        private ILogger<EshopHttpClient> Log { get; }

        public EshopHttpClient(ILogger<EshopHttpClient> log)
            : base(new HttpClientHandler {Proxy = null, UseProxy = false})
        {
            Log = log;
            Timeout = TimeSpan.FromSeconds(120);
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T?> GetAs<T>(string url) where T : class
        {
            var response = await GetBody(url);

            if (response != null)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(response);
                }
                catch (Exception e)
                {
                    Log.LogWarning(e, "Failed to deserialize response as {type}", typeof(T).FullName);
                }
            }

            return null;
        }

        private async Task<string?> GetBody(string url)
        {
            try
            {
                var response = await GetAsync(url).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    return body;
                }
            }
            catch (Exception e)
            {
                Log.LogWarning(e, "Failed HTTP request {url}", url);
            }

            return null;
        }
    }
}