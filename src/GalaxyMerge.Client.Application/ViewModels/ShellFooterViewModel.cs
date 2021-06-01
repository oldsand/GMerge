using System.Threading;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Core.Logging;
using NLog;
using Prism.Commands;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public class ShellFooterViewModel : ViewModelBase
    {
        private LogEventInfo _previousLogEventInfo;
        private string _previousLogMessage;
        private bool _showEventLog;
        private DelegateCommand _showHideEventLogCommand;


        public ShellFooterViewModel()
        {
            var logTarget = LogManager.Configuration.FindTargetByName<MemoryEventTarget>("NotificationTarget");
            logTarget.EventReceived += LogEventReceived;
        }

        private CancellationTokenSource CurrentCancellationTokenSource { get; set; }


        public string PreviousLogMessage
        {
            get => _previousLogMessage;
            private set => SetProperty(ref _previousLogMessage, value);
        }

        public bool ShowEventLog
        {
            get => _showEventLog;
            set => SetProperty(ref _showEventLog, value);
        }

        public LogEventInfo PreviousLogEventInfo
        {
            get => _previousLogEventInfo;
            set => SetProperty(ref _previousLogEventInfo, value);
        }

        public DelegateCommand ShowHideEventLogCommand =>
            _showHideEventLogCommand ??= new DelegateCommand(ExecuteShowHideEventLogCommand);

        private void LogEventReceived(LogEventInfo logInfo)
        {
            PreviousLogEventInfo = logInfo;
            PreviousLogMessage = logInfo.FormattedMessage;
        }

        private void ExecuteShowHideEventLogCommand()
        {
            ShowEventLog = !ShowEventLog;
        }
    }
}