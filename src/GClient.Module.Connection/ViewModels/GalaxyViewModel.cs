using System;
using GClient.Core.Mvvm;
using GClient.Core.Naming;
using GClient.Module.Connection.Utilities;
using GClient.Wrappers;
using Prism.Regions;

namespace GClient.Module.Connection.ViewModels
{
    public class GalaxyViewModel : NavigationViewModelBase
    {
        private ResourceEntryWrapper _resourceEntry;
        public override bool CreateScopedRegionManager => true;

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var resource = navigationContext.Parameters.GetValue<ResourceEntryWrapper>("resource");
            _resourceEntry = resource ?? throw new ArgumentNullException(nameof(resource));
            
            RegionManager.RequestNavigate(RegionName.NavigationRegion, ScopedNames.GalaxyTreeView, navigationContext.Parameters);
        }
    }
}