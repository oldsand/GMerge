using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Wrappers;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using NLog;
using Prism.Commands;
using Prism.Regions;

namespace GalaxyMerge.Client.UI.Connection.ViewModels
{
    public class GalaxyTreeViewModel : NavigationViewModelBase
    {
        private readonly IGalaxyDataRepositoryFactory _galaxyDataRepositoryFactory;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private ResourceEntryWrapper _resourceEntry;
        private string _connectionString;
        private DelegateCommand<GObject> _openObjectViewCommand;

        public GalaxyTreeViewModel()
        {
        }

        public GalaxyTreeViewModel(IGalaxyDataRepositoryFactory galaxyDataRepositoryFactory)
        {
            Derivations = new ObservableCollection<GObject>();
            _galaxyDataRepositoryFactory = galaxyDataRepositoryFactory;
        }

        public ObservableCollection<GObject> Derivations { get; }
        
        public DelegateCommand<GObject> OpenObjectViewCommand =>
            _openObjectViewCommand ??= new DelegateCommand<GObject>(ExecuteOpenObjectViewCommand, CanExecuteOpenObjectViewCommand);

        private void ExecuteOpenObjectViewCommand(GObject gObject)
        {
            Logger.Trace("Opening GalaxyObject Vie for object '{ObjectName}'", gObject.TagName);
            //RegionManager.RequestNavigate();
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

            LoadAsync().Await(OnLoadComplete, OnLoadError);
        }

        protected override async Task LoadAsync()
        {
            Logger.Debug("Loading data from object repository with connection string '{ConnectionString}'", _connectionString);
            Loading = true;
            
            var dataRepository = _galaxyDataRepositoryFactory.Create(_connectionString);
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
            Logger.Error(ex, "Fialed to load required data for galaxy tree view");
            Loading = false;
        }
    }
}