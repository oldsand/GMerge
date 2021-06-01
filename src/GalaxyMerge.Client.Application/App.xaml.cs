using System.Windows;
using GalaxyMerge.Client.Application.Views;
using GalaxyMerge.Client.Core.Prism;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GalaxyMerge.Client.Application
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            //LoggerConfig.Run();
            return Container.Resolve<Shell>();
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IRegionNavigationContentLoader, ScopedRegionNavigationContentLoader>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            regionBehaviors.AddIfMissing(RegionManagerAwareBehavior.BehaviorKey, typeof(RegionManagerAwareBehavior));
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
        }
    }
}