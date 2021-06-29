using System;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers.Base;
using NLog;

namespace GalaxyMerge.Client.Wrappers
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