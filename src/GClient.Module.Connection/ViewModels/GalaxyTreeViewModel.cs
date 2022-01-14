using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GClient.Core.Mvvm;
using GClient.Core.Naming;
using GClient.Core.Prism;
using GClient.Wrappers;
using GClient.Module.Connection.Utilities;
using GCommon.Data.Abstractions;
using GCommon.Data.Entities;
using NLog;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace GClient.Module.Connection.ViewModels
{
    public class GalaxyTreeViewModel : NavigationViewModelBase
    {
        private readonly IGalaxyDataProviderFactory _galaxyDataProviderFactory;
        private readonly IDialogService _dialogService;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private ResourceEntryWrapper _resourceEntry;
        private string _connectionString;
        private DelegateCommand<GalaxyObject> _openObjectViewCommand;

        public GalaxyTreeViewModel()
        {
        }

        public GalaxyTreeViewModel(IGalaxyDataProviderFactory galaxyDataProviderFactory,
            IDialogService dialogService)
        {
            Derivations = new ObservableCollection<GalaxyObject>();
            _galaxyDataProviderFactory = galaxyDataProviderFactory;
            _dialogService = dialogService;
        }

        public ObservableCollection<GalaxyObject> Derivations { get; }

        public DelegateCommand<GalaxyObject> OpenObjectViewCommand =>
            _openObjectViewCommand ??=
                new DelegateCommand<GalaxyObject>(ExecuteOpenObjectViewCommand, CanExecuteOpenObjectViewCommand);

        private void ExecuteOpenObjectViewCommand(GalaxyObject galaxyObject)
        {
            Logger.Trace("Opening GalaxyObject Vie for object '{ObjectName}'", galaxyObject.TagName);
            var parameters = new NavigationParameters {{"object", galaxyObject}};
            RegionManager.RequestNavigate(RegionName.ContentRegion, ScopedNames.GalaxyObjectView, parameters);
        }

        private bool CanExecuteOpenObjectViewCommand(GalaxyObject galaxyObject)
        {
            return true;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Logger.Trace("Navigated to GalaxyTreeView. Initializing view model parameters");

            var resource = navigationContext.Parameters.GetValue<ResourceEntryWrapper>("resource");
            _resourceEntry = resource ?? throw new ArgumentNullException(nameof(resource), @"Resource cannot be null");
            _connectionString = _resourceEntry.Connection.GetConnectionString();

            LoadData();
        }

        private void LoadData()
        {
            LoadAsync().Await(OnLoadComplete, OnLoadError);
        }

        protected override async Task LoadAsync()
        {
            Logger.Debug("Loading data from object repository with connection string '{ConnectionString}'",
                _connectionString);
            Loading = true;

            var dataRepository = _galaxyDataProviderFactory.Create(_connectionString);
            var derivations = (await dataRepository.Objects.GetDerivationHierarchy()).ToList();

            Derivations.Clear();
            Derivations.AddRange(derivations);
        }

        protected override void OnLoadComplete()
        {
            Logger.Debug("Load Galaxy Objects Complete");
            Loading = false;
        }

        protected override void OnLoadError(Exception ex)
        {
            Loading = false;
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                var errorMessage =
                    $"Failed to load data from galaxy '{_resourceEntry.Connection.GalaxyName}' on host '{_resourceEntry.Connection.NodeName}'";
                
                Logger.Error(ex, errorMessage);
                
                _dialogService.ShowError("Object Load Failure", errorMessage,
                    ex,
                    result =>
                    {
                        if (result.Result == ButtonResult.Retry)
                            LoadData();
                    });
            });
        }
    }
}