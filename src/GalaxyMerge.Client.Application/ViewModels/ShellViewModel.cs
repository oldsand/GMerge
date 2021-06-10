using GalaxyMerge.Client.Application.Views;
using GalaxyMerge.Client.Core.Mvvm;
using NLog;
using Prism.Mvvm;
using Prism.Regions;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public ShellViewModel()
        {
        }

        public ShellViewModel(IRegionManager regionManager)
        {
            Logger.Trace("Initializing Shell ViewModel");
            regionManager.RegisterViewWithRegion(RegionName.ShellHeaderRegion, typeof(ShellHeaderView));
            regionManager.RegisterViewWithRegion(RegionName.ShellFooterRegion, typeof(ShellFooterView));
        }
    }
}