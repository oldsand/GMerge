using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.UI.Connection.Utilities;
using GalaxyMerge.Client.Wrappers;
using Prism.Regions;

namespace GalaxyMerge.Client.UI.Connection.ViewModels
{
    public class ConnectionViewModel : NavigationViewModelBase
    {
        
        
        public ConnectionViewModel()
        {
        }

        public override bool CreateScopedRegionManager => true;

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var resource = navigationContext.Parameters.GetValue<ResourceEntryWrapper>("resource"); 
            
            RegionManager.RegisterViewWithRegion(ScopedNames.GalaxyRegion, ScopedNames.GalaxyView);
            RegionManager.RequestNavigate(ScopedNames.ChangeLogRegion, ScopedNames.ChangeLogView, navigationContext.Parameters);
            RegionManager.RequestNavigate(ScopedNames.ArchiveRegion, ScopedNames.ArchiveView, navigationContext.Parameters);
            RegionManager.RequestNavigate(ScopedNames.GalaxyRegion, ScopedNames.GalaxyView, GalaxyViewNavigationComplete, navigationContext.Parameters);
        }

        private void GalaxyViewNavigationComplete(NavigationResult obj)
        {
            //no longer loading?
        }
    }
}