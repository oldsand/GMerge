using System.Windows;
using System.Windows.Navigation;
using GalaxyMerge.Client.Application.Configurations;
using GalaxyMerge.Client.Application.ViewModels;
using GalaxyMerge.Client.Application.Views;
using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.Core.Prism;
using GalaxyMerge.Client.Core.Prism.RegionBehaviors;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Repositories;
using GalaxyMerge.Client.Dialogs;
using GalaxyMerge.Client.UI.Connection;
using GalaxyMerge.Data.Abstractions;
using NLog;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GalaxyMerge.Client.Application
{
    public partial class App
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        
        protected override Window CreateShell()
        {
            LoggerConfiguration.Apply();
            Logger.Debug("Log Configuration Applied - Creating new Application Shell");
            return Container.Resolve<Shell>();
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IRegionNavigationContentLoader, ScopedRegionNavigationContentLoader>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IGalaxyDataRepositoryFactory, GalaxyDataRepositoryFactory>();
            containerRegistry.Register<IResourceRepository, ResourceRepository>();
            containerRegistry.Register<ILogRepository, LogRepository>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<DialogModule>();
            moduleCatalog.AddModule<ConnectionModule>();
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            regionBehaviors.AddIfMissing(RegionManagerAwareBehavior.BehaviorKey, typeof(RegionManagerAwareBehavior));
            regionBehaviors.AddIfMissing(DependentViewRegionBehavior.BehaviorKey, typeof(DependentViewRegionBehavior));
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
        }
    }
}