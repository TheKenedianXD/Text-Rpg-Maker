using System.Reflection;
using TextRpg.Core.Models.Config;
using TextRpg.Core.Models.Enums;
using TextRpg.Core.Services.Data;

namespace TextRpg.Core.Utilities
{
    public static class Logger
    {
        private static AppConfigModel _config = new();

        public static bool IsInitialized { get; private set; } = false;

        public static void Initialize(AppConfigModel config)
        {
            _config = config;
            IsInitialized = true;
            LogInfo($"{nameof(Logger)}::{nameof(Initialize)}", "Logger initialized with AppConfig.");
        }

        private static readonly Dictionary<string, int> LogLevels = new()
        {
            { "Info", 1 },
            { "Warning", 2 },
            { "Error", 3 }
        };

        private static string GetLogFilePath()
        {
            string baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            Directory.CreateDirectory(baseDirectory);

            string logFileName = $"log-{DateTime.Now:ddMMyyyy}.log";
            return Path.Combine(baseDirectory, logFileName);
        }

        private static void WriteLog(string level, string origin, string message, Exception? exception = null)
        {
            if (!IsInitialized) return;

            if (LogLevels[level] < LogLevels[_config.LogLevel])
                return;

            string timestamp = DateTime.Now.ToString("dd.MM.yyyy-HH:mm:ss");
            string logMessage = $"[{level}] {timestamp} | {origin} | {message}";

            if (exception != null)
            {
                logMessage += $" | Exception: {exception.Message} | {exception.StackTrace}";
                if (exception.InnerException != null)
                {
                    logMessage += $" | InnerException: {exception.InnerException.Message} | {exception.InnerException.StackTrace}";
                }
            }

            try
            {
                File.AppendAllText(GetLogFilePath(), logMessage + Environment.NewLine);
            } catch (Exception logEx)
            {
                Console.WriteLine($"[Logger] Failed to write log: {logEx.Message}");
            }
        }

        public static void LogInfo(string origin, string message) => WriteLog("Info", origin, message);
        public static void LogWarning(string origin, string message) => WriteLog("Warning", origin, message);
        public static void LogError(string origin, string message, Exception? exception = null) => WriteLog("Error", origin, message, exception);
    }
}
