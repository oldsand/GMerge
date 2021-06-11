using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Core.Logging;
using ImTools;
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
        private readonly MemoryEventTarget _logTarget;


        public ShellFooterViewModel()
        {
            _logTarget = LogManager.Configuration.FindTargetByName<MemoryEventTarget>("NotificationTarget");
            _logTarget.EventReceived += LogEventReceived;
        }

        public override void Destroy()
        {
            _logTarget.EventReceived -= LogEventReceived;
        }

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
            PreviousLogMessage = $"{logInfo.FormattedMessage} at {logInfo.TimeStamp.ToLongTimeString()}";
        }

        private void ExecuteShowHideEventLogCommand()
        {
            ShowEventLog = !ShowEventLog;
        }
    }
}