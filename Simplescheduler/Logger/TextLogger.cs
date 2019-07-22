using log4net;
using System;
using System.Diagnostics;
using System.Text;

namespace Simplescheduler.Logger
{
    public enum LogType
    {
        Error = 1,
        Warning = 2,
        Info = 3,
        Debug = 4,
        CriticalError = 5,
    }

    public interface ILogger
    {
        void Log(string message, LogType logType);
        void Log(string message, LogType logType, Exception ex);
    }


    public class TextLogger : ILogger
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TextLogger));

        /// <summary>
        /// Identifier to track the related records
        /// </summary>
        public string TrackId { get; set; }

        private string GetFileName(LogType logType)
        {
            switch (logType)
            {
                case LogType.Error://ErrorLogFile
                    return System.Configuration.ConfigurationManager.AppSettings[LogType.Error.ToString() + "LogFile"];
                case LogType.Debug://DebugLogFile
                    return System.Configuration.ConfigurationManager.AppSettings[LogType.Debug.ToString() + "LogFile"];
                case LogType.Warning://WarningLogFile
                    return System.Configuration.ConfigurationManager.AppSettings[LogType.Warning.ToString() + "LogFile"];
                case LogType.Info://InfoLogFile
                    return System.Configuration.ConfigurationManager.AppSettings[LogType.Info.ToString() + "LogFile"];
                case LogType.CriticalError://CriticalErrorLogFile
                    return System.Configuration.ConfigurationManager.AppSettings[LogType.CriticalError.ToString() + "LogFile"];
            }

            return "";

        }

        public TextLogger(string uniqueId)
        {
            TrackId = $"{uniqueId}_{DateTime.UtcNow.ToString("Hmmss")}";//UserId_123555
            //For now only single file will be used to track all type of logging
            log4net.GlobalContext.Properties["FileName"] = GetFileName(LogType.Error) + DateTime.Now.ToString("dd_MM_yyyy");
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Log(string message, LogType logType)
        {
            string logSeparator = Environment.NewLine + "-----------------------------------------------------------" + Environment.NewLine;

            message = message + logSeparator;
            switch (logType)
            {
                case LogType.Debug:
                    logger.Debug(message);
                    break;
                case LogType.Error:
                    logger.Error(message);
                    break;
                case LogType.Info:
                    logger.Info(message);
                    break;
                case LogType.Warning:
                    logger.Warn(message);
                    break;
                default:
                    logger.Error(message);
                    break;
            }
        }

        //TODO concept of writing log in single file by multiple processes
        public void Log(string message, LogType logType, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            var lineNumber = new StackTrace(ex, true).GetFrame(0).GetFileLineNumber();

            sb.Append($"=========================={logType.ToString()}- TrackId: {TrackId }==========================" + Environment.NewLine);
            sb.Append($" Time: {DateTime.UtcNow.ToString("hh:mm tt") + Environment.NewLine}");
            sb.Append($" Message:{ ex?.Message + Environment.NewLine} at line no.={lineNumber}");
            sb.Append($" Error: { ex + Environment.NewLine}");
            sb.Append("==========================End- TrackId: {TrackId }============================" + Environment.NewLine);

            switch (logType)
            {
                case LogType.Debug:
                    logger.Debug(sb.ToString());
                    break;
                case LogType.Error:
                    logger.Error(sb.ToString());
                    break;
                case LogType.Info:
                    logger.Info(sb.ToString());
                    break;
                case LogType.Warning:
                    logger.Warn(sb.ToString());
                    break;
                default:
                    logger.Error(sb.ToString());
                    break;
            }
        }
    }
}