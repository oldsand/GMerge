using System;
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
        private ResourceType _selectedResourceType;
        private string _resourceName;
        private string _resourceDescription;
        private string _nodeName;
        private string _galaxyName;
        private string _fileName;
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
        }

        public override void OnDialogClosed()
        {
            _resourceRepository.Dispose();
        }

        public ResourceType SelectedResourceType
        {
            get => _selectedResourceType;
            set
            {
                SetProperty(ref _selectedResourceType, value);
                CanExecuteBackCommand();
            }
        }

        public string ResourceName
        {
            get => _resourceName;
            set => SetProperty(ref _resourceName, value);
        }

        public string ResourceDescription
        {
            get => _resourceDescription;
            set => SetProperty(ref _resourceDescription, value);
        }

        public string NodeName
        {
            get => _nodeName;
            set => SetProperty(ref _nodeName, value);
        }

        public string GalaxyName
        {
            get => _galaxyName;
            set => SetProperty(ref _galaxyName, value);
        }

        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        public DelegateCommand<string> ResourceTypeSelectedCommand =>
            _resourceTypeSelectedCommand ??= new DelegateCommand<string>(ExecuteResourceTypeSelectedCommand);
        
        private void ExecuteResourceTypeSelectedCommand(string resourceTypeName)
        {
            SelectedResourceType = (ResourceType) Enum.Parse(typeof(ResourceType), resourceTypeName);
        }

        public DelegateCommand BackCommand =>
            _backCommand ??= new DelegateCommand(ExecuteBackCommand, CanExecuteBackCommand);

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

            var galaxyResource = new ResourceEntry(_resourceName, _selectedResourceType);

            try
            {
                _resourceRepository.Add(galaxyResource);
                _resourceRepository.Save();
                Logger.Info("Added new {ResourceType} resource named {ResourceName}", _selectedResourceType,
                    _resourceName);

                _eventAggregator.GetEvent<RefreshResourcesEvent>().Publish();

                ExecuteCloseDialog();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to add new {ResourceType} resource named {ResourceName}", _selectedResourceType,
                    _resourceName);
            }
        }

        private bool CanExecuteSaveResourceCommand()
        {
            return true;
        }
    }
}