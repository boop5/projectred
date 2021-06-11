using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Red.Core.Application.Interfaces;

namespace Red.Infrastructure.Logging
{
    internal sealed class AppLogger<T> : IAppLogger<T> 
        where T : class
    {
        public AppLogger(ILoggerFactory loggerFactory)
        {
            Log = loggerFactory.CreateLogger<T>();
        }

        private ILogger Log { get; }

        internal AppLogger(ILogger<T> log)
        {
            Log = log;
        }

        public void LogTrace(Exception exception, string message, params object[] args)
        {
            Log.LogTrace(exception.Demystify(), message, args);
        }

        public void LogTrace(string message, params object[] args)
        {
            Log.LogTrace(message, args);
        }

        public void LogDebug(Exception exception, string message, params object[] args)
        {
            Log.LogDebug(exception.Demystify(), message, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            Log.LogDebug(message, args);
        }

        public void LogInformation(Exception exception, string message, params object[] args)
        {
            Log.LogInformation(exception.Demystify(), message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            Log.LogInformation(message, args);
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            Log.LogWarning(exception.Demystify(), message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            Log.LogWarning(message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            Log.LogError(exception.Demystify(), message, args);
        }

        public void LogError(string message, params object[] args)
        {
            Log.LogError(message, args);
        }

        public void LogCritical(Exception exception, string message, params object[] args)
        {
            Log.LogCritical(exception.Demystify(), message, args);
        }

        public void LogCritical(string message, params object[] args)
        {
            Log.LogCritical(message, args);
        }
    }
}