using System.Collections.Generic;
using System.Globalization;

namespace Red.Infrastructure.Spider.Settings
{
    internal sealed class WorkerSettings
    {
        public LibrarySpiderConfiguration LibrarySpider { get; init; } = new();
        public MediaSpiderConfiguration MediaSpider { get; init; } = new();
        public PriceSpiderConfiguration PriceSpider { get; init; } = new();
        public SalesSpiderConfiguration SalesSpider { get; init; } = new();

        public List<CultureInfo> Cultures { get; init; } = new();
    }
}