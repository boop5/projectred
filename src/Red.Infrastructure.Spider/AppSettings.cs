using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Red.Infrastructure.Spider
{
    internal class SpiderConfiguration
    {
        public int Interval { get; init; }
        public int Delay { get; init; }
    }

    internal sealed class LibrarySpiderConfiguration : SpiderConfiguration { }
    internal sealed class PriceSpiderConfiguration : SpiderConfiguration { }
    internal sealed class SalesSpiderConfiguration : SpiderConfiguration { }
    internal sealed class MediaSpiderConfiguration : SpiderConfiguration { }

    internal sealed class AppSettings
    {
        [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global", Justification = "Updated by IConfiguration.Bind()")]
        public Dictionary<string, SpiderConfiguration> Workers { get; init; } = new();
    }
}
