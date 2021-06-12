using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Events;
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
        private readonly IResourceRepository _resourceRepository;
        private ResourceEntry _selectedResourceEntry;
        private ObservableCollection<ResourceEntry> _resources;
        private DelegateCommand _addResourceCommand;
        private DelegateCommand _deleteResourceCommand;
        
        public ShellHeaderViewModel()
        {
            
        }

        public ShellHeaderViewModel(IEventAggregator eventAggregator, IDialogService dialogService,
            IResourceRepository resourceRepository)
        {
            Logger.Trace("Initializing Shell Header ViewModel");
            
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _resourceRepository = resourceRepository;

            Resources = new ObservableCollection<ResourceEntry>();
            
            eventAggregator.GetEvent<RefreshResourcesEvent>().Subscribe(OnRefreshResources);
            LoadResources().Await(ResourceLoadComplete, ResourceLoadError);
        }
        

        public ResourceEntry SelectedResourceEntry
        {
            get => _selectedResourceEntry;
            set => SetProperty(ref _selectedResourceEntry, value);
        }
        
        public ObservableCollection<ResourceEntry> Resources
        {
            get => _resources;
            set => SetProperty(ref _resources, value);
        }

        public DelegateCommand NewResourceCommand => _addResourceCommand ??= new DelegateCommand(ExecuteNewResourceCommand);
        
        private void ExecuteNewResourceCommand()
        {
            Logger.Trace("Executing New Resource Command. Showing Dialog {DialogName}", DialogName.NewResourceDialog);
            _dialogService.Show(DialogName.NewResourceDialog, result => { });
        }
        
        public DelegateCommand DeleteResourceCommand =>
            _deleteResourceCommand ??= new DelegateCommand(ExecuteDeleteResourceCommand, CanExecuteDeleteResourceCommand);
        
        private void ExecuteDeleteResourceCommand()
        {
            //todo
        }

        private bool CanExecuteDeleteResourceCommand()
        {
            return true;
        }

        private async Task LoadResources()
        {
            Logger.Trace("Getting all resource records from application database");
            
            var resources = await _resourceRepository.GetAllAsync();

            if (resources != null)
            {
                Resources.Clear();
                Resources.AddRange(resources);   
            }
        }
        
        private void ResourceLoadError(Exception ex)
        {
            Logger.Error(ex, "Unable to load galaxy resources from application data store");
            
            //todo display generic error message box to user
        }
        
        private void ResourceLoadComplete()
        {
            if (Resources.Count > 0)
            {
                Logger.Info("{ResourceCount} Resources Loaded Successfully", Resources.Count);
                return;
            }
            
            Logger.Info("No Resources Found");
        }
        
        private void OnRefreshResources()
        {
            LoadResources().Await(ResourceLoadComplete, ResourceLoadError);
        }
    }
}