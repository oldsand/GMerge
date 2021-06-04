using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public class ShellHeaderViewModel : ViewModelBase
    {
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
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
        }

        public int SelectedResource
        {
            get => _selectedResource;
            set => SetProperty(ref _selectedResource, value);
        }

        public DelegateCommand AddResourceCommand => _addResourceCommand ??= new DelegateCommand(ExecuteAddResource);
        
        public DelegateCommand DeleteResourceCommand =>
            _deleteResourceCommand ??= new DelegateCommand(ExecuteDeleteResource, CanExecuteDeleteResource);

        private void ExecuteAddResource()
        {
            _dialogService.Show(DialogName.AddResourceDialog, result => { });
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