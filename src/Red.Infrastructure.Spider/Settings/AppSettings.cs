namespace Red.Infrastructure.Spider.Settings
{
    internal sealed class AppSettings
    {
        public WorkerSettings Workers { get; init; } = new();
    }
}