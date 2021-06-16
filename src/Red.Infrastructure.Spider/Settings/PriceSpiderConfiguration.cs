namespace Red.Infrastructure.Spider.Settings
{
    internal sealed class PriceSpiderConfiguration : SpiderConfiguration
    {
        public int ChunkCount { get; init; } = 10;
    }
}