using System;
using System.Runtime.CompilerServices;
using NLog;

// ReSharper disable UnusedAutoPropertyAccessor.Local
[assembly: InternalsVisibleTo("GalaxyMerge.Client.Data.Tests")]
namespace GalaxyMerge.Client.Data.Entities
{
    public class LogEntry
    {
        private LogEntry()
        {
        }

        internal LogEntry(string message)
        {
            
        }

        public int LogId { get; private set; }
        public DateTime Logged { get; private set; }
        public int LevelId { get; private set; }
        public string Level { get; private set; }
        public LogLevel LogLevel => LogLevel.FromOrdinal(LevelId);
        public string Message { get; private set; }
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