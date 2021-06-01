using System.Threading;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Core.Logging;
using NLog;
using Prism.Commands;

namespace GalaxyMerge.ViewModels
{
    public class ShellFooterViewModel : ViewModelBase
    {
        private string _displayLogMessage;
        private bool _displayNotification;
        private LogLevel _displayLogLevel;
        private DelegateCommand _closeNotificationCommand;


        public ShellFooterViewModel()
        {
            var logTarget = LogManager.Configuration.FindTargetByName<MemoryEventTarget>("NotificationTarget");
            logTarget.EventReceived += LogEventReceived;
        }

        private CancellationTokenSource CurrentCancellationTokenSource { get; set; }
        
        public string DisplayLogMessage
        {
            get => _displayLogMessage;
            private set => SetProperty(ref _displayLogMessage, value);
        }

        public LogLevel DisplayLogLevel
        {
            get => _displayLogLevel;
            set => SetProperty(ref _displayLogLevel, value);
        }

        public bool DisplayNotification
        {
            get => _displayNotification;
            set => SetProperty(ref _displayNotification, value);
        }

        public DelegateCommand CloseNotificationCommand =>
            _closeNotificationCommand ??= new DelegateCommand(ExecuteCloseNotificationCommand);

        private DelegateCommand _cancelCurrentTaskCommand;

        public DelegateCommand CancelCurrentTaskCommand =>
            _cancelCurrentTaskCommand ??= new DelegateCommand(ExecuteCancelCurrentTaskCommand);

        private void ExecuteCancelCurrentTaskCommand()
        {
            CurrentCancellationTokenSource.Cancel();
        }

        private void LogEventReceived(LogEventInfo logInfo)
        {
            DisplayLogMessage = logInfo.FormattedMessage;
            DisplayLogLevel = logInfo.Level;
        }

        private void ExecuteCloseNotificationCommand()
        {
            DisplayNotification = false;
        }
    }
}