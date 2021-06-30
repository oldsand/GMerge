using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.UI.Connection.Utilities;
using Prism.Regions;

namespace GalaxyMerge.Client.UI.Connection.ViewModels
{
    public class GalaxyViewModel : NavigationViewModelBase
    {
        public override bool CreateScopedRegionManager => true;
        
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            RegionManager.RequestNavigate(RegionName.NavigationRegion, ScopedNames.GalaxyTreeView, navigationContext.Parameters);
        }
    }
}