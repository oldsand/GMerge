using System.Collections.ObjectModel;
using GalaxyMerge.Client.Application.ViewModels;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers;
using GalaxyMerge.Core.Logging;
using NLog;

namespace GalaxyMerge.Client.Application.Design
{
    public class DesignModels
    {
        public static ShellHeaderViewModel ShellHeaderViewModel=> new ShellHeaderViewModel();
        public static ResourceEntryWrapper ResourceEntry => new(new ResourceEntry("ResourceName", ResourceType.Connection));

        public static EventLogViewModel EventLogViewModel => new()
        {
            Logs = new ObservableCollection<LogEventWrapper>()
            {
                new(new LogEventInfo(LogLevel.Info, LoggerName.NotificationLogger, "This is a Log Message")),
                new(new LogEventInfo(LogLevel.Warn, LoggerName.NotificationLogger, "This is an error message. Something went wrong")),
                new(new LogEventInfo(LogLevel.Error, LoggerName.NotificationLogger, "This is an warning message. Something is not doing alright but we still good")),
                new(new LogEventInfo(LogLevel.Info, LoggerName.NotificationLogger, "This is a Log Message")),
                new(new LogEventInfo(LogLevel.Info, LoggerName.NotificationLogger, "This is a Log Message"))
            }
        };
        
        public static LogEventWrapper LogEventWrapper => new(new LogEventInfo(LogLevel.Info, LoggerName.NotificationTarget, "This is a test message for a log event entry"));
    }
}