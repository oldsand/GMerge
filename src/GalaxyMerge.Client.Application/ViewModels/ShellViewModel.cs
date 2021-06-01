using GalaxyMerge.Client.Application.Views;
using GalaxyMerge.Client.Core.Mvvm;
using Prism.Mvvm;
using Prism.Regions;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        public ShellViewModel()
        {
        }

        public ShellViewModel(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(RegionName.ShellHeaderRegion, typeof(ShellHeaderView));
            regionManager.RegisterViewWithRegion(RegionName.ShellFooterRegion, typeof(ShellFooterView));
        }
    }
}