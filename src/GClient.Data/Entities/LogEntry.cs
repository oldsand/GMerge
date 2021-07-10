using System;
using System.Runtime.CompilerServices;
using NLog;

namespace GClient.Data.Entities
{
    public class LogEntry
    {
        private LogEntry()
        {
        }

        public LogEntry(LogLevel level, string message)
        {
            Logged = DateTime.Now;
            LevelId = level.Ordinal;
            Message = message;
            MachineName = Environment.MachineName;
            Identity = Environment.UserName;
        }
        
        public LogEntry(LogEventInfo logEventInfo)
        {
            if (logEventInfo == null)
                throw new ArgumentNullException(nameof(logEventInfo), "LogEventInfo cannot be null");
            
            LogId = logEventInfo.SequenceID;
            Logged = logEventInfo.TimeStamp;
            LevelId = logEventInfo.Level.Ordinal;
            Level = logEventInfo.Level.Name;
            Message = logEventInfo.Message;
            FormattedMessage = logEventInfo.FormattedMessage;
            Logger = logEventInfo.LoggerName;
            Properties = logEventInfo.Properties.ToString();
            Callsite = logEventInfo.CallerMemberName;
            FileName = logEventInfo.CallerFilePath;
            LineNumber = logEventInfo.CallerLineNumber;
            Stacktrace = logEventInfo.StackTrace.ToString();
            MachineName = Environment.MachineName;
            Identity = Environment.UserName;
            Exception = logEventInfo.Exception?.ToString();
            
        }

        public int LogId { get; private set; }
        public DateTime Logged { get; private set; }
        public int LevelId { get; private set; }
        public string Level { get; private set; }
        public LogLevel LogLevel => LogLevel.FromOrdinal(LevelId);
        public string Message { get; private set; }
        public string FormattedMessage { get; private set; }
        public string Logger { get; private set; }
        public string Properties { get; private set; }
        public string Callsite { get; private set; }
        public string FileName { get; private set; }
        public int LineNumber { get; private set; }
        public string Stacktrace { get; private set; }
        public string MachineName { get; private set; }
        public string Identity { get; private set; }
        public string Exception { get; private set; }
    }
}