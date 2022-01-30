using System;

namespace HomeAssistantTaskbarWidget
{
    public interface ILogger
    {
        Action<string> OnError { get; set; }

        void ChangeLogLevel(LogLevel logLevel);

        void LogDebug(string message);
        void LogDebug(Exception exception);
        void LogDebug(string message, Exception exception);
        void LogInfo(string message);
        void LogInfo(Exception exception);
        void LogInfo(string message, Exception exception);
        void LogWarn(string message);
        void LogWarn(Exception exception);
        void LogWarn(string message, Exception exception);
        void LogError(string message);
        void LogError(Exception exception);
        void LogError(string message, Exception exception);

    }
}