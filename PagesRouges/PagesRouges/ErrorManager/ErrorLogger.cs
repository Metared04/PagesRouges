using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagesRouges.ErrorManager
{
    public static class ErrorLogger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "error.log");
        static ErrorLogger()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
        }
        public static void LogError(Exception ex, string context = "")
        {
            try
            {
                var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR";
                if (!string.IsNullOrEmpty(context))
                    logEntry += $" [{context}]";

                logEntry += $": {ex.Message}";
                if (ex.InnerException != null)
                    logEntry += $" | Inner: {ex.InnerException.Message}";

                logEntry += $" | StackTrace: {ex.StackTrace}";
                logEntry += Environment.NewLine;

                lock (typeof(ErrorLogger))
                {
                    File.AppendAllText(LogFilePath, logEntry);
                }
            }
            catch
            {
                
            }
        }
        public static void LogError(string message, string context = "")
        {
            try
            {
                var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR";
                if (!string.IsNullOrEmpty(context))
                    logEntry += $" [{context}]";

                logEntry += $": {message}{Environment.NewLine}";

                lock (typeof(ErrorLogger))
                {
                    File.AppendAllText(LogFilePath, logEntry);
                }
            }
            catch
            {
                
            }
        }
    }
}
