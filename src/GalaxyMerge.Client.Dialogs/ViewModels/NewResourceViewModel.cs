using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers;
using NLog;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class NewResourceViewModel : DialogViewModelBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IResourceRepository _resourceRepository;
        private readonly List<string> _resourceNames = new List<string>();
        private ResourceType _selectedResourceType;
        private DelegateCommand<string> _resourceTypeSelectedCommand;
        private DelegateCommand _backCommand;
        private DelegateCommand _saveResourceCommand;


        public NewResourceViewModel()
        {
        }

        public NewResourceViewModel(IResourceRepository resourceRepository)
        {
            if (resourceRepository == null)
                throw new ArgumentNullException();

            Logger.Trace("Initializing New Resource Dialog");
            _resourceRepository = resourceRepository;

            Logger.Trace("Loading current resource names");
            _resourceNames = _resourceRepository.GetNames().ToList();
        }

        public override void OnDialogClosed()
        {
            _resourceRepository.Dispose();
        }

        public ResourceEntryWrapper ResourceEntry { get; set; }

        public ResourceType SelectedResourceType
        {
            get => _selectedResourceType;
            set => SetProperty(ref _selectedResourceType, value);
        }

        public DelegateCommand<string> ResourceTypeSelectedCommand =>
            _resourceTypeSelectedCommand ??= new DelegateCommand<string>(ExecuteResourceTypeSelectedCommand);
        
        private void ExecuteResourceTypeSelectedCommand(string resourceTypeName)
        {
            SelectedResourceType = (ResourceType) Enum.Parse(typeof(ResourceType), resourceTypeName);
            var entry = new ResourceEntry("", SelectedResourceType);
            ResourceEntry = new ResourceEntryWrapper(entry);
        }

        public DelegateCommand BackCommand =>
            _backCommand ??= new DelegateCommand(ExecuteBackCommand, CanExecuteBackCommand)
                .ObservesProperty(() => SelectedResourceType);

        private void ExecuteBackCommand()
        {
            SelectedResourceType = ResourceType.None;
        }

        private bool CanExecuteBackCommand()
        {
            return SelectedResourceType != ResourceType.None;
        }

        public DelegateCommand SaveResourceCommand =>
            _saveResourceCommand ??= new DelegateCommand(ExecuteSaveResourceCommand, CanExecuteSaveResourceCommand);

        private void ExecuteSaveResourceCommand()
        {
            Logger.Trace("Executing Save New Resource Command");

            try
            {
                _resourceRepository.Add(ResourceEntry.Model);
                _resourceRepository.Save();
                
                Logger.Info("Added new {ResourceType} resource named {ResourceName}", ResourceEntry.ResourceType,
                    ResourceEntry.ResourceName);

                var parameters = new DialogParameters {{"resource", ResourceEntry}};
                RaiseRequestClose(new DialogResult(ButtonResult.OK, parameters));
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to add new {ResourceType} resource named {ResourceName}", ResourceEntry.ResourceType,
                    ResourceEntry.ResourceName);
                
                //todo prompt user?
            }
        }

        private bool CanExecuteSaveResourceCommand()
        {
            return ResourceEntry.IsValid;
        }
    }
}