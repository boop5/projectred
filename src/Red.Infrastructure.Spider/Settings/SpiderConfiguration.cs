namespace Red.Infrastructure.Spider.Settings
{
    internal abstract class SpiderConfiguration
    {
        public int Delay { get; init; } = 10;
        public int Interval { get; init; } = 30;
    }
}