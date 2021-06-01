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
            /*containerRegistry.RegisterSingleton<IGalaxyConnectionRegistry, GalaxyConnectionRegistry>();
            containerRegistry.RegisterSingleton<ITaskProcessor, TaskProcessor>();
            containerRegistry.Register<IGalaxyViewManager, GalaxyViewManager>();
            containerRegistry.Register<IGalaxyDatabaseInspector, GalaxyDatabaseInspector>();
            containerRegistry.Register<IGalaxyDataProviderFactory, GalaxyDataProviderFactory>();
            containerRegistry.Register<IConnectionRepository, ConnectionRepository>();
            containerRegistry.RegisterInstance<DbContext>(new VcsContext());*/
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            /*moduleCatalog.AddModule<ConnectionModule>();
            moduleCatalog.AddModule<RepositoryModule>();*/
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            regionBehaviors.AddIfMissing(RegionManagerAwareBehavior.BehaviorKey, typeof(RegionManagerAwareBehavior));
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
        }
    }
}