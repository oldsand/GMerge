using GClient.Core.Naming;
using GClient.Module.Connection.Utilities;
using GClient.Module.Connection.ViewModels;
using GClient.Module.Connection.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace GClient.Module.Connection
{
    public class ConnectionModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ConnectionView, ConnectionViewModel>(ViewName.ConnectionView);
            containerRegistry.RegisterForNavigation<GalaxyView, GalaxyViewModel>(ScopedNames.GalaxyView);
            containerRegistry.RegisterForNavigation<ChangeLogView, ChangeLogViewModel>(ScopedNames.ChangeLogView);
            containerRegistry.RegisterForNavigation<ArchiveView, ArchiveViewModel>(ScopedNames.ArchiveView);
            containerRegistry.RegisterForNavigation<GalaxyTreeView, GalaxyTreeViewModel>(ScopedNames.GalaxyTreeView);
            containerRegistry.RegisterForNavigation<GalaxyObjectView, GalaxyObjectViewModel>(ScopedNames.GalaxyObjectView);
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}