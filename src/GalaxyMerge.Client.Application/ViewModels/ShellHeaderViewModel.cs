using GalaxyMerge.Client.Core.Mvvm;
using Prism.Commands;
using Prism.Events;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public class ShellHeaderViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private int _selectedResource;
        private DelegateCommand _addResourceCommand;
        private DelegateCommand _deleteResourceCommand;

        public ShellHeaderViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
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
            //todo
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