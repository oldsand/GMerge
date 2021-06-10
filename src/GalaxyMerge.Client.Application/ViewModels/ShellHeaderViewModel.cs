using GalaxyMerge.Client.Core.Mvvm;
using NLog;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public class ShellHeaderViewModel : ViewModelBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private int _selectedResource;
        private DelegateCommand _addResourceCommand;
        private DelegateCommand _deleteResourceCommand;

        public ShellHeaderViewModel()
        {
            
        }

        public ShellHeaderViewModel(IEventAggregator eventAggregator, IDialogService dialogService) 
        {
            Logger.Trace("Initializing Shell Header ViewModel");
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
        }

        public int SelectedResource
        {
            get => _selectedResource;
            set => SetProperty(ref _selectedResource, value);
        }

        public DelegateCommand NewResourceCommand => _addResourceCommand ??= new DelegateCommand(ExecuteAddResource);
        
        public DelegateCommand DeleteResourceCommand =>
            _deleteResourceCommand ??= new DelegateCommand(ExecuteDeleteResource, CanExecuteDeleteResource);

        private void ExecuteAddResource()
        {
            Logger.Trace("Executing New Resource Command. Showing Dialog {DialogName}", DialogName.NewResourceDialog);
            _dialogService.Show(DialogName.NewResourceDialog, result => { });
        }

        private void ExecuteDeleteResource()
        {
            //todo
        }

        private bool CanExecuteDeleteResource()
        {
            return true;
        }
    }
}