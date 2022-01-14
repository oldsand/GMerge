using System.Collections.ObjectModel;
using GClient.Application.ViewModels;
using GClient.Data.Entities;
using GClient.Wrappers;
using NLog;

namespace GClient.Application.Design
{
    public class DesignModels
    {
        public static ShellHeaderViewModel ShellHeaderViewModel=> new ShellHeaderViewModel();
        public static ResourceEntryWrapper ResourceEntry => new(new ResourceEntry("ResourceName", ResourceType.Connection));

        public static EventLogViewModel EventLogViewModel => new()
        {
            Logs = new ObservableCollection<LogEntryWrapper>()
            {
                new(new LogEntry(LogLevel.Info, "This is a Log Message")),
                new(new LogEntry(LogLevel.Warn, "This is an error message. Something went wrong")),
                new(new LogEntry(LogLevel.Error, "This is an warning message. Something is not doing alright but we still good")),
                new(new LogEntry(LogLevel.Info, "This is a Log Message")),
                new(new LogEntry(LogLevel.Info, "This is a Log Message"))
            }
        };
        
        public static LogEntryWrapper LogEntryWrapper => new(new LogEntry(LogLevel.Info, "This is a test message for a log event entry"));
    }
}