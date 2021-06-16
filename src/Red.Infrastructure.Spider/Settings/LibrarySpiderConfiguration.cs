namespace Red.Infrastructure.Spider.Settings
{
    internal sealed class LibrarySpiderConfiguration : SpiderConfiguration
    {
        public int QuerySize { get; init; } = 200;
    }
}