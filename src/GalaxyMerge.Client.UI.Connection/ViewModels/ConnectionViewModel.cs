using System;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.UI.Connection.Utilities;
using GalaxyMerge.Client.Wrappers;
using Prism.Regions;

namespace GalaxyMerge.Client.UI.Connection.ViewModels
{
    public class ConnectionViewModel : NavigationViewModelBase
    {
        private ResourceEntryWrapper _resourceEntry;

        public ConnectionViewModel()
        {
        }

        public ResourceEntryWrapper ResourceEntry
        {
            get => _resourceEntry;
            set => SetProperty(ref _resourceEntry, value);
        }

        public override bool CreateScopedRegionManager => true;

        public override bool KeepAlive => false;

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var resource = navigationContext.Parameters.GetValue<ResourceEntryWrapper>("resource");
            ResourceEntry = resource ?? throw new ArgumentNullException(nameof(resource));
            
            RegionManager.RequestNavigate(ScopedNames.ChangeLogRegion, ScopedNames.ChangeLogView, navigationContext.Parameters);
            RegionManager.RequestNavigate(ScopedNames.ArchiveRegion, ScopedNames.ArchiveView, navigationContext.Parameters);
            RegionManager.RequestNavigate(ScopedNames.GalaxyRegion, ScopedNames.GalaxyView, navigationContext.Parameters);
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var resource = navigationContext.Parameters.GetValue<ResourceEntryWrapper>("resource");
            return resource.Model.ResourceId == ResourceEntry.Model.ResourceId;
        }
    }
}