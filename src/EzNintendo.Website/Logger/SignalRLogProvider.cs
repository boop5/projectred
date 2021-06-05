using System;
using Microsoft.Extensions.Logging;

namespace EzNintendo.Website.Logger
{
    public class SignalRLogProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        //private readonly ConcurrentDictionary<string, SignalRLogger> _loggers = new ConcurrentDictionary<string, SignalRLogger>();
        private SignalRLogger _logger;

        public SignalRLogProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            _logger = null;
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (_logger == null)
            {
                _logger = new SignalRLogger(_serviceProvider);
            }

            return _logger;
        }
    }
}