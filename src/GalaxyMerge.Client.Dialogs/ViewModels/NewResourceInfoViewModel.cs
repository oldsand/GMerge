using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Dialogs.Commands;
using GalaxyMerge.Client.Events;
using GalaxyMerge.Client.Wrappers;
using NLog;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class NewResourceInfoViewModel : NavigationViewModelBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IResourceRepository _resourceRepository;
        private readonly IDialogCommands _dialogCommands;
        private readonly IEventAggregator _eventAggregator;
        private List<string> _resourceNames;
        private ResourceEntryWrapper _resourceEntry;
        private DelegateCommand _saveResourceCommand;
        private IRegionNavigationService _navigationService;

        public NewResourceInfoViewModel()
        {
        }

        public NewResourceInfoViewModel(IResourceRepository resourceRepository, IDialogCommands dialogCommands, IEventAggregator eventAggregator)
        {
            if (resourceRepository == null)
                throw new ArgumentNullException();

            Logger.Trace("Initializing New Resource Dialog");
            _resourceRepository = resourceRepository;
            _dialogCommands = dialogCommands;
            _eventAggregator = eventAggregator;

            _dialogCommands.SaveCommand.RegisterCommand(SaveResourceCommand);
            _dialogCommands.BackCommand.RegisterCommand(ReturnToSelectionCommand);
        }
        

        public ResourceEntryWrapper ResourceEntry
        {
            get => _resourceEntry;
            private set => SetProperty(ref _resourceEntry, value);
        }

        public DelegateCommand SaveResourceCommand => 
            _saveResourceCommand ??= new DelegateCommand(ExecuteSaveResourceCommand, CanExecuteSaveResourceCommand)
                .ObservesProperty(() => ResourceEntry.IsValid)
                .ObservesProperty(() => ResourceEntry.HasRequired);

        private DelegateCommand _returnToSelectionCommand;

        public DelegateCommand ReturnToSelectionCommand =>
            _returnToSelectionCommand ??= new DelegateCommand(ExecuteReturnToSelectionCommand);

        private void ExecuteReturnToSelectionCommand()
        {
            if (_navigationService.Journal.CanGoBack)
                _navigationService.Journal.GoBack();
        }

        private void ExecuteSaveResourceCommand()
        {
            Logger.Trace("Executing Save New Resource Command");

            try
            {
                _resourceRepository.Add(ResourceEntry.Model);
                _resourceRepository.Save();

                Logger.Info("Added new {ResourceType} resource named {ResourceName}", ResourceEntry.ResourceType,
                    ResourceEntry.ResourceName);

                var args = new NewResourceCompleteEventArgs(ButtonResult.OK, ResourceEntry);
                _eventAggregator.GetEvent<NewResourceCompleteEvent>().Publish(args);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to add new {ResourceType} resource named {ResourceName}",
                    ResourceEntry.ResourceType,
                    ResourceEntry.ResourceName);

                var args = new NewResourceCompleteEventArgs(ButtonResult.Abort, ResourceEntry);
                _eventAggregator.GetEvent<NewResourceCompleteEvent>().Publish(args);
            }
        }

        private bool CanExecuteSaveResourceCommand()
        {
            return ResourceEntry is {IsValid: true, HasRequired: false};
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationService = navigationContext.NavigationService;

            var resourceType = navigationContext.Parameters.GetValue<ResourceType>("resourceType");
            var entry = new ResourceEntry("", resourceType);

            LoadAsync().Await(OnLoadComplete, OnLoadError);

            ResourceEntry = new ResourceEntryWrapper(entry, _resourceNames);
        }

        protected override async Task LoadAsync()
        {
            Logger.Trace("Loading current resource names");
            _resourceNames = (await _resourceRepository.GetNamesAsync()).ToList();
            Logger.Trace("Loaded {ResourceNameCount} names from database", _resourceNames.Count);
        }

        protected override void OnLoadError(Exception ex)
        {
            Logger.Error(ex, "Failed to load resource names from database");

            if (ex is DbException)
            {
            }

            //Present Error to user
        }

        public override bool KeepAlive => false;

        public override void Destroy()
        {
            _resourceRepository.Dispose();
            _dialogCommands.SaveCommand.UnregisterCommand(SaveResourceCommand);
            _dialogCommands.BackCommand.UnregisterCommand(ReturnToSelectionCommand);
        }
    }
}