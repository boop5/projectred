using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace EzNintendo.Website.Services.Media
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated by Runtime.")]
    public sealed class ImageService
    {
        private readonly ILogger<ImageService> _log;
        private readonly IFileSystem _fs;

        public ImageService(ILogger<ImageService> log, IFileSystem fs)
        {
            _log = log;
            _fs = fs;

            _log.LogTrace("Instance created.");
        }

        public async Task MinifyAsync(string source, string destination, int quality = 75)
        {
            _log.LogDebug("Minify image from {source} to {destination}", source, destination);

            await Task.Run(() =>
            {
                Minify(source, destination, quality);
            });
        }

        public void Minify(string source, string destination, int quality = 75)
        {
            try
            {
                var encoder = new JpegEncoder { Quality = quality, Subsample = JpegSubsample.Ratio444 };
                using var image = Image.Load(source);
                using var stream = _fs.File.Create(destination);

                image.SaveAsJpeg(stream, encoder);
            }
            catch (Exception e)
            {
                _log.LogWarning(e, "Failed to minify image {source} {destination}", source, destination);
            }
        }

        public bool HealthCheck(string img)
        {
            try
            {
                Image.Load(img).Size();
            }
            catch (Exception e)
            {
                _log.LogWarning(e, "Image seems to be corrupt {image}", img);
                return false;
            }

            return true;
        }
    }
}