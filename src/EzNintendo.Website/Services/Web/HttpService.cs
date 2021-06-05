using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EzNintendo.Common.Extensions.System;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Website.Services.Web
{
    /// <summary>
    ///     A Collection of Helper-Methods for common HTTP Actions.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instance created by runtime.")]
    public sealed class HttpService
    {
        private readonly ILogger<HttpService> _log;

        public HttpService(ILogger<HttpService> log)
        {
            _log = log;
        }

        /// <summary>
        ///     Gets the Response from an HTTP-GET Request asynchronously.
        /// </summary>
        /// <param name="url">
        ///     The URL to fetch data from.
        /// </param>
        /// <returns>
        ///     Returns the Response from the Request.
        /// </returns>
        public async Task<string> GetAsync(string url)
        {
            _log.LogTrace($"GETAsync {url}");

            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36";
                request.Method = WebRequestMethods.Http.Get;
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using var response = await request.GetResponseAsync();
                await using var stream = response.GetResponseStream();
                using var reader = new StreamReader(stream ?? throw new NullReferenceException());
                var result = await reader.ReadToEndAsync();

                _log.LogDebug("Response for {url}: {response}", url, result.Truncate(10000));

                return result;
            }
            catch (Exception e)
            {
                _log.LogWarning(e, "Failed to GETAsync the requested resource {resource}", url);
                throw;
            }
        }

        public async Task DownloadFileAsync(string url, string localPath)
        {
            _log.LogDebug("DownloadFileAsync {url} {localPath}", url, localPath);

            using var webClient = new WebClient(); // todo: UserAgent??

            try
            {
                await webClient.DownloadFileTaskAsync(url, localPath);
            }
            catch (Exception e)
            {
                _log.LogWarning(e, "Failed to Download File from {url} to {localPath}", url, localPath);
            }
        }
    }
}