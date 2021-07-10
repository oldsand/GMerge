using System.Windows;
using Prism.Regions;

namespace GClient.Core.Prism
{
    public static class RegionManagerAware
    {
        public static void SetRegionManagerAware(object item, IRegionManager regionManager)
        {
            switch (item)
            {
                case IRegionManagerAware regionManagerAware:
                    regionManagerAware.RegionManager = regionManager;
                    break;
                case FrameworkElement frameworkElement:
                {
                    if (frameworkElement.DataContext is IRegionManagerAware elementRegionManagerAware)
                        elementRegionManagerAware.RegionManager = regionManager;
                    break;
                }
            }
        }
    }
}