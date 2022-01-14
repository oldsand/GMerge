using System.Collections.ObjectModel;
using GClient.Core.Mvvm;
using GClient.Data.Entities;
using GClient.Wrappers;
using GCommon.Logging;
using NLog;
using Prism.Commands;

namespace GClient.Application.ViewModels
{
    public class EventLogViewModel : ViewModelBase
    {
        private ObservableCollection<LogEntryWrapper> _logs;
        private DelegateCommand _clearLogsCommand;

        public EventLogViewModel()
        {
            Title = "Event Log";
            Logs = new ObservableCollection<LogEntryWrapper>();

            var target = LogManager.Configuration.FindTargetByName<MemoryEventTarget>(LoggerName.NotificationTarget);
            target.EventReceived += LogEventReceived;
        }

        public ObservableCollection<LogEntryWrapper> Logs
        {
            get => _logs;
            set => SetProperty(ref _logs, value);
        }

        public DelegateCommand ClearLogsCommand =>
            _clearLogsCommand ??= new DelegateCommand(ExecuteClearLogsCommand, CanExecuteClearLogsCommand);

        private void ExecuteClearLogsCommand()
        {
            Logs.Clear();
        }

        private bool CanExecuteClearLogsCommand()
        {
            return Logs.Count > 0;
        }

        private void LogEventReceived(LogEventInfo log)
        {
            var entry = new LogEntry(log);
            var wrapper = new LogEntryWrapper(entry);

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (Logs.Count >= 50) 
                    Logs.RemoveAt(Logs.Count - 1);
                
                Logs.Add(wrapper);
            });
        }
    }
}