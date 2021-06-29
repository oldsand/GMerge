using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.Events;
using GalaxyMerge.Core.Logging;
using ImTools;
using NLog;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public class ShellFooterViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private string _previousLogMessage;
        private bool _showEventLog;
        private DelegateCommand _showHideEventLogCommand;
        private readonly MemoryEventTarget _logTarget;

        public ShellFooterViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _logTarget = LogManager.Configuration.FindTargetByName<MemoryEventTarget>(LoggerName.NotificationTarget);
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
            set => SetProperty(ref _showEventLog, value, OnShowEventLogChanged);
        }

        public DelegateCommand ShowHideEventLogCommand =>
            _showHideEventLogCommand ??= new DelegateCommand(ExecuteShowHideEventLogCommand);

        private void LogEventReceived(LogEventInfo logInfo)
        {
            PreviousLogMessage = $"{logInfo.FormattedMessage} at {logInfo.TimeStamp.ToLongTimeString()}";
        }

        private void ExecuteShowHideEventLogCommand()
        {
            ShowEventLog = !ShowEventLog;
        }

        private void OnShowEventLogChanged()
        {
            _eventAggregator.GetEvent<ShowEventLogChangedEvent>().Publish(ShowEventLog);
        }
    }
}