using System.Collections.ObjectModel;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Wrappers;
using GalaxyMerge.Core.Logging;
using NLog;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public class EventLogViewModel : ViewModelBase
    {
        private ObservableCollection<LogEventWrapper> _logs;

        public EventLogViewModel()
        {
            Title = "Event Log";
            Logs = new ObservableCollection<LogEventWrapper>();

            var target = LogManager.Configuration.FindTargetByName<MemoryEventTarget>(LoggerName.NotificationTarget);
            target.EventReceived += LogEventReceived;
        }

        public ObservableCollection<LogEventWrapper> Logs
        {
            get => _logs;
            set => SetProperty(ref _logs, value);
        }

        private void LogEventReceived(LogEventInfo log)
        {
            var wrapper = new LogEventWrapper(log);
            Logs.Add(wrapper);
        }
    }
}