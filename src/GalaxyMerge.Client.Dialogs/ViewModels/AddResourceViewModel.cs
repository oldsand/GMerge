using System;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Entities;
using Prism.Commands;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class AddResourceViewModel : DialogViewModelBase
    {
        private readonly IResourceRepository _resourceRepository;
        private int _selectedResourceIndex;
        private string _resourceName;
        private string _nodeName;
        private string _galaxyName;
        private string _fileName;
        private int _currentStep;
        private DelegateCommand _saveResourceCommand;
        private DelegateCommand _nextStepCommand;
        private bool _showResourceSelection;
        private bool _showConnectionResource;
        private bool _showArchiveResource;
        private bool _showFileResource;


        public AddResourceViewModel()
        {
            _currentStep = 0;
            ProcessCurrentStep(_currentStep);
        }

        public AddResourceViewModel(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
            _currentStep = 0;
            ProcessCurrentStep(_currentStep);
        }

        public int SelectedResourceIndex
        {
            get => _selectedResourceIndex;
            set
            {
                SetProperty(ref _selectedResourceIndex, value);
                CanExecuteNextStepCommand();
            }
        }

        public string ResourceName
        {
            get => _resourceName;
            set
            {
                SetProperty(ref _resourceName, value);
                CanExecuteNextStepCommand();
            }
        }

        public string NodeName
        {
            get => _nodeName;
            set
            {
                SetProperty(ref _nodeName, value);
                CanExecuteNextStepCommand();
            }
        }

        public string GalaxyName
        {
            get => _galaxyName;
            set
            {
                SetProperty(ref _galaxyName, value);
                CanExecuteNextStepCommand();
            }
        }

        public string FileName
        {
            get => _fileName;
            set
            {
                SetProperty(ref _fileName, value);
                CanExecuteNextStepCommand();
            }
        }

        public bool ShowResourceSelection
        {
            get => _showResourceSelection;
            set => SetProperty(ref _showResourceSelection, value);
        }

        public bool ShowConnectionResource
        {
            get => _showConnectionResource;
            set => SetProperty(ref _showConnectionResource, value);
        }

        public bool ShowArchiveResource
        {
            get => _showArchiveResource;
            set => SetProperty(ref _showArchiveResource, value);
        }

        public bool ShowFileResource
        {
            get => _showFileResource;
            set => SetProperty(ref _showFileResource, value);
        }

        public DelegateCommand SaveResourceCommand =>
            _saveResourceCommand ??= new DelegateCommand(ExecuteSaveResourceCommand, CanExecuteSaveResourceCommand);

        public DelegateCommand NextStepCommand =>
            _nextStepCommand ??= new DelegateCommand(ExecuteNextStepCommand, CanExecuteNextStepCommand);

        private DelegateCommand _previousStepCommand;

        public DelegateCommand PreviousStepCommand =>
            _previousStepCommand ??= new DelegateCommand(ExecutePreviousStepCommand);

        private void ExecutePreviousStepCommand()
        {
            _currentStep--;
            ProcessCurrentStep(_currentStep);
        }

        private void ExecuteNextStepCommand()
        {
            _currentStep++;
            ProcessCurrentStep(_currentStep);
            CanExecuteNextStepCommand();
        }

        private void ProcessCurrentStep(int currentStep)
        {
            ShowResourceSelection = _currentStep == 0;
            ShowConnectionResource = _currentStep == 1 && SelectedResourceIndex == 0;
            ShowArchiveResource = _currentStep == 1 && SelectedResourceIndex == 1;
            ShowFileResource = _currentStep == 1 && SelectedResourceIndex == 2;
        }

        private bool CanExecuteNextStepCommand()
        {
            if (_currentStep == 0)
                return SelectedResourceIndex >= 0;

            if (_currentStep == 1)
            {
                return !string.IsNullOrEmpty(ResourceName) && ((!string.IsNullOrEmpty(NodeName) && !string.IsNullOrEmpty(GalaxyName))
                       || !string.IsNullOrEmpty(FileName));
            }

            return false;
        }

        private void ExecuteSaveResourceCommand()
        {
            var resourceType = (ResourceType) _selectedResourceIndex;
            var galaxyResource = new GalaxyResource(_resourceName, resourceType, _nodeName, _galaxyName, _fileName);
            _resourceRepository.Add(galaxyResource);
            _resourceRepository.Save();
        }

        private bool CanExecuteSaveResourceCommand()
        {
            return true;
        }
    }
}