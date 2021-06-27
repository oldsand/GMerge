using GalaxyMerge.Client.Application.Views;
using GalaxyMerge.Client.Core.Naming;
using NLog;
using Prism.Mvvm;
using Prism.Regions;

namespace GalaxyMerge.Client.Application.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public ShellViewModel(IRegionManager regionManager)
        {
            Logger.Trace("Initializing Shell ViewModel");
            regionManager.RegisterViewWithRegion<ShellHeaderView>(RegionName.ShellHeaderRegion);
            regionManager.RegisterViewWithRegion<ShellFooterView>(RegionName.ShellFooterRegion);
        }
    }
}