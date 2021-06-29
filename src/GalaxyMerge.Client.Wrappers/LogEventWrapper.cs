using System;
using GalaxyMerge.Client.Wrappers.Base;
using NLog;

namespace GalaxyMerge.Client.Wrappers
{
    public class LogEventWrapper : ModelWrapper<LogEventInfo>
    {
        public LogEventWrapper(LogEventInfo model, bool callInitialize = true) : base(model, callInitialize)
        {
        }

        public DateTime TimeStamp
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public string FormattedMessage
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public LogLevel Level
        {
            get => GetValue<LogLevel>();
            set => SetValue(value);
        }
    }
}