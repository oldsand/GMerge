using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Entities;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public class EventLogViewModel : ViewModelBase
    {
        private readonly ILogRepository _logRepository;
        private ObservableCollection<LogEntry> _logs;

        public EventLogViewModel(ILogRepository logRepository)
        {
            _logRepository = logRepository;
            Logs = new ObservableCollection<LogEntry>();
            LoadAsync().Await(OnLoadComplete, OnLoadError);
        }

        public ObservableCollection<LogEntry> Logs
        {
            get => _logs;
            set => SetProperty(ref _logs, value);
        }

        protected override async Task LoadAsync()
        {
            var logs = await _logRepository.GetEventLogsAsync();
            Logs.Clear();
            Logs.AddRange(logs);
        }
    }
}