using System;
using System.IO;

namespace HomeAssistantTaskbarWidget
{
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warn = 2,
        Error = 3
    }

    public class Logger : ILogger
    {
        private string _logFormat = "[{0}] - {1} => {2} \n";

        private string _filePath = "log.txt";

        private LogLevel _logLevel;

        public Action<string> OnError { get; set; }

        public Logger()
        {
        }

        public Logger(string filePath)
        {
            _filePath = filePath;
        }

        public Logger(string filePath, LogLevel logLevel)
        {
            _filePath = filePath;
            _logLevel = logLevel;
        }

        public Logger(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }

        public void ChangeLogLevel(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }


        private void Log(LogLevel level, string message)
        {
            if (_logLevel <= level)
            {
                var formattedMessage = string.Format(_logFormat, DateTime.Now.ToString(),
                    Enum.GetName(typeof(LogLevel), level)?.ToUpper(), message);

                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

                File.AppendAllText(_filePath, formattedMessage);
            }

            if(level == LogLevel.Error)
                OnError?.Invoke(message);
        }

        public void LogDebug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void LogDebug(Exception exception)
        {
            LogDebug(exception.ToString());
        }

        public void LogDebug(string message, Exception exception)
        {
            LogDebug($"{message} \n Exception: \n {exception.ToString()}");
        }

        public void LogInfo(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void LogInfo(Exception exception)
        {
            LogInfo(exception.ToString());
        }

        public void LogInfo(string message, Exception exception)
        {
            LogInfo($"{message} \n Exception: \n {exception.ToString()}");
        }

        public void LogWarn(string message)
        {
            Log(LogLevel.Warn, message);
        }

        public void LogWarn(Exception exception)
        {
            LogWarn(exception.ToString());
        }

        public void LogWarn(string message, Exception exception)
        {
            LogWarn($"{message} \n Exception: \n {exception.ToString()}");
        }

        public void LogError(string message)
        {
            Log(LogLevel.Error, message);
        }

        public void LogError(Exception exception)
        {
            LogError(exception.ToString());
        }

        public void LogError(string message, Exception exception)
        {
            LogError($"{message} \n Exception: \n {exception.ToString()}");
        }
    }
}
