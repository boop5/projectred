using System;
using System.Diagnostics.CodeAnalysis;

namespace Red.Core.Application.Interfaces
{
    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface IAppLogger<T> : IAppLogger where T : class
    {

    }
    public interface IAppLogger
    {
        void LogTrace(Exception exception, string message, params object[] args);
        void LogTrace(string message, params object[] args);

        void LogDebug(Exception exception, string message, params object[] args);
        void LogDebug(string message, params object[] args);

        void LogInformation(Exception exception, string message, params object[] args);
        void LogInformation(string message, params object[] args);

        void LogWarning(Exception exception, string message, params object[] args);
        void LogWarning(string message, params object[] args);

        void LogError(Exception exception, string message, params object[] args);
        void LogError(string message, params object[] args);

        void LogCritical(Exception exception, string message, params object[] args);
        void LogCritical(string message, params object[] args);
    }
}