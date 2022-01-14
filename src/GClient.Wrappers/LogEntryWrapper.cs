using System;
using GClient.Data.Entities;
using GClient.Wrappers.Base;
using NLog;

namespace GClient.Wrappers
{
    public class LogEntryWrapper : ModelWrapper<LogEntry>
    {
        public LogEntryWrapper(LogEntry model) : base(model, false)
        {
        }

        public DateTime Logged
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public LogLevel LogLevel
        {
            get => GetValue<LogLevel>();
            set => SetValue(value);
        }

        public string FormattedMessage
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}