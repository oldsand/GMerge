using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Views;
using Prism.Mvvm;
using Prism.Regions;

namespace GalaxyMerge.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        public ShellViewModel()
        {
        }

        public ShellViewModel(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(RegionName.ShellHeaderRegion, typeof(ShellHeaderView));
        }
    }
}