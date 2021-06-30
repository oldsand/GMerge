using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Wrappers;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using Microsoft.Data.SqlClient;
using NLog;
using Prism.Regions;

namespace GalaxyMerge.Client.UI.Connection.ViewModels
{
    public class GalaxyTreeViewModel : NavigationViewModelBase
    {
        private readonly IDataRepositoryFactory _repositoryFactory;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private ResourceEntryWrapper _resourceEntry;
        private SqlConnectionStringBuilder _connectionStringBuilder;

        public GalaxyTreeViewModel()
        {
        }

        public GalaxyTreeViewModel(IDataRepositoryFactory repositoryFactory)
        {
            Derivations = new ObservableCollection<GObject>();
            _repositoryFactory = repositoryFactory;
        }

        public ObservableCollection<GObject> Derivations { get; }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Logger.Trace("Navigated to GalaxyTreeView. Initializing view model parameters");
            var resource = navigationContext.Parameters.GetValue<ResourceEntryWrapper>("resource");
            _resourceEntry = resource ?? throw new ArgumentNullException(nameof(resource), @"Resource cannot be null");
            _connectionStringBuilder = new SqlConnectionStringBuilder(
                DbStringBuilder.BuildGalaxy(_resourceEntry.Connection.NodeName, _resourceEntry.Connection.GalaxyName));

            LoadAsync().Await(OnLoadComplete, OnLoadError);
        }

        protected override async Task LoadAsync()
        {
            Logger.Debug("Loading data from object repository with connection string '{ConnectionString}'",
                _connectionStringBuilder.ConnectionString);

            var objectRepository = _repositoryFactory.Create<GObject, IObjectRepository>(_connectionStringBuilder);
            var derivations = (await objectRepository.GetDerivationHierarchy()).ToList();
            
            Derivations.Clear();
            Derivations.AddRange(derivations);
        }

        protected override void OnLoadComplete()
        {
            Logger.Debug("Load Galaxy Objects Complete");
        }

        protected override void OnLoadError(Exception ex)
        {
            Logger.Error(ex, "Fialed to load required data for galaxy tree view");
        }
    }
}