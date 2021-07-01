using System;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.UI.Connection.Utilities;
using GalaxyMerge.Client.Wrappers;
using Prism.Regions;

namespace GalaxyMerge.Client.UI.Connection.ViewModels
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