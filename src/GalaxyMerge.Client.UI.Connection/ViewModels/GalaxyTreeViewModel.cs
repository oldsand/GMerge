using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.Core.Prism;
using GalaxyMerge.Client.UI.Connection.Utilities;
using GalaxyMerge.Client.Wrappers;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using NLog;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.UI.Connection.ViewModels
{
    public class GalaxyTreeViewModel : NavigationViewModelBase
    {
        private readonly IGalaxyDataProviderFactory _galaxyDataProviderFactory;
        private readonly IDialogService _dialogService;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private ResourceEntryWrapper _resourceEntry;
        private string _connectionString;
        private DelegateCommand<GObject> _openObjectViewCommand;

        public GalaxyTreeViewModel()
        {
        }

        public GalaxyTreeViewModel(IGalaxyDataProviderFactory galaxyDataProviderFactory,
            IDialogService dialogService)
        {
            Derivations = new ObservableCollection<GObject>();
            _galaxyDataProviderFactory = galaxyDataProviderFactory;
            _dialogService = dialogService;
        }

        public ObservableCollection<GObject> Derivations { get; }

        public DelegateCommand<GObject> OpenObjectViewCommand =>
            _openObjectViewCommand ??=
                new DelegateCommand<GObject>(ExecuteOpenObjectViewCommand, CanExecuteOpenObjectViewCommand);

        private void ExecuteOpenObjectViewCommand(GObject gObject)
        {
            Logger.Trace("Opening GalaxyObject Vie for object '{ObjectName}'", gObject.TagName);
            var parameters = new NavigationParameters {{"object", gObject}};
            RegionManager.RequestNavigate(RegionName.ContentRegion, ScopedNames.GalaxyObjectView, parameters);
        }

        private bool CanExecuteOpenObjectViewCommand(GObject gObject)
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