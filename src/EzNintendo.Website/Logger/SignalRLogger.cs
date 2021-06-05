using System;
using EzNintendo.Website.WebSocketHubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Website.Logger
{
    public class SignalRLogger : ILogger
    {
        private readonly IServiceProvider _serviceProvider;
        private IHubContext<LogHub> _logHub;

        private IHubContext<LogHub> LogHub
        {
            get
            {
                if (_logHub == null)
                {
                    _logHub = _serviceProvider.GetService<IHubContext<LogHub>>();
                }

                return _logHub;
            }
        }


        public SignalRLogger(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var msg = $"[{logLevel}] {formatter(state, exception)}";
            var hub = _serviceProvider.GetService<IHubContext<LogHub>>();

            _serviceProvider.GetService<IHubContext<LogHub>>()?.Clients.All.SendAsync("xdd");

            if (hub != null)
            {
                LogHub.Clients.All.SendAsync(msg);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true; // todo: use logLevel to check IsEnabled bruh
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}