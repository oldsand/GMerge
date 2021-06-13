using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Events;
using NLog;
using Prism.Commands;
using Prism.Events;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class NewResourceViewModel : DialogViewModelBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IResourceRepository _resourceRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly List<string> _resourceNames = new List<string>();
        private ResourceType _selectedResourceType;
        private string _resourceName;
        private string _resourceDescription;
        private string _nodeName;
        private string _galaxyName;
        private string _fileName;
        private string _directoryName;
        private DelegateCommand<string> _resourceTypeSelectedCommand;
        private DelegateCommand _backCommand;
        private DelegateCommand _saveResourceCommand;


        public NewResourceViewModel()
        {
        }

        public NewResourceViewModel(IResourceRepository resourceRepository, IEventAggregator eventAggregator)
        {
            if (resourceRepository == null)
                throw new ArgumentNullException();

            Logger.Trace("Initializing New Resource Dialog");
            _resourceRepository = resourceRepository;
            _eventAggregator = eventAggregator;

            Logger.Trace("Loading current resource names");
            _resourceNames = _resourceRepository.GetNames().ToList();
        }

        public override void OnDialogClosed()
        {
            _resourceRepository.Dispose();
        }

        public ResourceType SelectedResourceType
        {
            get => _selectedResourceType;
            set => SetProperty(ref _selectedResourceType, value);
        }

        public string ResourceName
        {
            get => _resourceName;
            set
            {
                SetProperty(ref _resourceName, value);
                SaveResourceCommand.RaiseCanExecuteChanged();
            }
        }

        public string ResourceDescription
        {
            get => _resourceDescription;
            set => SetProperty(ref _resourceDescription, value);
        }

        public string NodeName
        {
            get => _nodeName;
            set
            {
                SetProperty(ref _nodeName, value);
                SaveResourceCommand.RaiseCanExecuteChanged();
            }
        }

        public string GalaxyName
        {
            get => _galaxyName;
            set
            {
                SetProperty(ref _galaxyName, value);
                SaveResourceCommand.RaiseCanExecuteChanged();
            }
        }

        public string FileName
        {
            get => _fileName;
            set
            {
                SetProperty(ref _fileName, value);
                SaveResourceCommand.RaiseCanExecuteChanged();
            }
        }

        public string DirectoryName
        {
            get => _directoryName;
            set
            {
                SetProperty(ref _directoryName, value);
                SaveResourceCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand<string> ResourceTypeSelectedCommand =>
            _resourceTypeSelectedCommand ??= new DelegateCommand<string>(ExecuteResourceTypeSelectedCommand);
        
        private void ExecuteResourceTypeSelectedCommand(string resourceTypeName)
        {
            SelectedResourceType = (ResourceType) Enum.Parse(typeof(ResourceType), resourceTypeName);
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

            var resourceEntry = new ResourceEntry(ResourceName, SelectedResourceType, ResourceDescription);
            resourceEntry = SetEntryInformation(resourceEntry);

            try
            {
                _resourceRepository.Add(resourceEntry);
                _resourceRepository.Save();
                
                Logger.Info("Added new {ResourceType} resource named {ResourceName}", SelectedResourceType,
                    ResourceName);

                _eventAggregator.GetEvent<NewResourceAddedEvent>().Publish(resourceEntry.ResourceName);

                ExecuteCancelDialog();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to add new {ResourceType} resource named {ResourceName}", _selectedResourceType,
                    _resourceName);
            }
        }

        private bool CanExecuteSaveResourceCommand()
        {
            return !string.IsNullOrEmpty(ResourceName)
                   && SelectedResourceType == ResourceType.Connection
                ? HasValidConnectionData()
                : SelectedResourceType == ResourceType.Archive
                    ? HasValidArchiveData()
                    : HasValidDirectoryData();
        }

        private bool HasValidConnectionData()
        {
            return !string.IsNullOrEmpty(NodeName)
                   && !string.IsNullOrEmpty(GalaxyName);
        }
        
        private bool HasValidArchiveData()
        {
            return !string.IsNullOrEmpty(FileName);
        }
        
        private bool HasValidDirectoryData()
        {
            return !string.IsNullOrEmpty(DirectoryName);
        }
        
        private ResourceEntry SetEntryInformation(ResourceEntry resourceEntry)
        {
            switch (resourceEntry.ResourceType)
            {
                case ResourceType.None:
                    break;
                case ResourceType.Connection:
                    resourceEntry.SetConnection(NodeName, GalaxyName);
                    break;
                case ResourceType.Archive:
                    resourceEntry.SetArchive(FileName);
                    break;
                case ResourceType.Directory:
                    resourceEntry.SetDirectory(DirectoryName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return resourceEntry;
        }
    }
}