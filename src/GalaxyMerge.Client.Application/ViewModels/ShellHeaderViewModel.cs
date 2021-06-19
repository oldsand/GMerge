using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.Core.Prism;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers;
using GalaxyMerge.Client.Wrappers.Base;
using NLog;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public sealed class ShellHeaderViewModel : ViewModelBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IDialogService _dialogService;
        private readonly IResourceRepository _resourceRepository;
        private ResourceEntryWrapper _selectedResourceEntry;
        private ChangeTrackingCollection<ResourceEntryWrapper> _resources;
        private DelegateCommand _addResourceCommand;
        private DelegateCommand _openSettingsCommand;
        private DelegateCommand _deleteResourceCommand;
        
        public ShellHeaderViewModel()
        {
        }

        public ShellHeaderViewModel(IDialogService dialogService,
            IResourceRepository resourceRepository)
        {
            Logger.Trace("Initializing Shell Header ViewModel");

            _dialogService = dialogService;
            _resourceRepository = resourceRepository;
            
            LoadAsync().Await(OnLoadComplete, OnLoadError);
        }

        public ResourceEntryWrapper SelectedResourceEntry
        {
            get => _selectedResourceEntry;
            set => SetProperty(ref _selectedResourceEntry, value);
        }

        public ChangeTrackingCollection<ResourceEntryWrapper> Resources
        {
            get => _resources;
            private set => SetProperty(ref _resources, value);
        }

        protected override async Task LoadAsync()
        {
            Logger.Trace("Getting all resource records from application database");

            var resources = (await _resourceRepository.GetAllAsync()).ToList();

            if (Resources != null)
            {
                Resources.Clear();
                Resources.AddRange(resources.Select(r => new ResourceEntryWrapper(r)));
                Resources.AcceptChanges();
                return;
            }
            
            Resources = new ChangeTrackingCollection<ResourceEntryWrapper>(resources
                .Select(r => new ResourceEntryWrapper(r)).ToList());
        }

        protected override void OnLoadError(Exception ex)
        {
            Logger.Error(ex, "Unable to load galaxy resources from application data store");

            //todo display generic error message box to user
        }

        protected override void OnLoadComplete()
        {
            if (Resources.Count > 0)
            {
                Logger.Info("{ResourceCount} Resource(s) loaded successfully", Resources.Count);
                return;
            }

            Logger.Info("No resources found");
        }

        public DelegateCommand NewResourceCommand =>
            _addResourceCommand ??= new DelegateCommand(ExecuteNewResourceCommand);

        private void ExecuteNewResourceCommand()
        {
            Logger.Trace("Executing new resource command. Showing dialog {DialogName}", DialogName.NewResourceDialog);
            
            _dialogService.Show(DialogName.NewResourceDialog, dialogResult =>
            {
                Logger.Trace("Entering new resource command callback with result {ButtonResult}", dialogResult.Result);
                if (dialogResult.Result != ButtonResult.OK) return;

                var resource = dialogResult.Parameters.GetValue<ResourceEntryWrapper>("resource");
                Resources.Add(resource);
                Resources.AcceptChanges();
                Logger.Trace("Added new resource {ResourceName} to resources collection", resource.ResourceName);
            });
        }

        public DelegateCommand OpenSettingsCommand =>
            _openSettingsCommand ??= new DelegateCommand(ExecuteOpenSettingsCommand, CanExecuteOpenSettingsCommand)
                .ObservesProperty(() => SelectedResourceEntry);

        private void ExecuteOpenSettingsCommand()
        {
            Logger.Trace("Executing open resource settings command for resource {ResourceName}",
                SelectedResourceEntry.ResourceName);
            
            var parameters = new DialogParameters {{"resource", SelectedResourceEntry}};
            
            _dialogService.Show(DialogName.ResourceSettingsDialog, parameters, _ => { });
        }

        private bool CanExecuteOpenSettingsCommand()
        {
            return SelectedResourceEntry != null;
        }

        public DelegateCommand DeleteResourceCommand =>
            _deleteResourceCommand ??=
                new DelegateCommand(ExecuteDeleteResourceCommand, CanExecuteDeleteResourceCommand)
                    .ObservesProperty(() => SelectedResourceEntry);

        private void ExecuteDeleteResourceCommand()
        {
            Logger.Trace("Executing delete resource command for resource {ResourceName}",
                SelectedResourceEntry.ResourceName);

            _dialogService.ShowConfirmation(
                $"Are you sure you want to delete the resource '{SelectedResourceEntry.ResourceName}'?",
                dialogResult =>
                {
                    Logger.Trace("Entering delete resource command callback with result {ButtonResult}", dialogResult.Result);
                    if (dialogResult.Result != ButtonResult.OK) return;

                    var resource = SelectedResourceEntry;
                    var resourceName = SelectedResourceEntry.ResourceName;

                    try
                    {
                        Logger.Trace("Deleting {ResourceName} from database", resourceName);
                        _resourceRepository.Remove(resource.Model);
                        _resourceRepository.Save();
                
                        Logger.Trace("Removing {ResourceName} from resource collection and setting selected resource to null", resourceName);
                        Resources.Remove(resource);
                        SelectedResourceEntry = null;
                        Resources.AcceptChanges();

                        Logger.Info("Resource {ResourceName} successfully removed", resourceName);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e, "Failed to remove {ResourceName}", resourceName);
                        //todo prompt user of error
                    }
                });
        }

        private bool CanExecuteDeleteResourceCommand()
        {
            return SelectedResourceEntry != null;
        }
    }
}