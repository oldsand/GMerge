using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.UI.Connection.ViewModels;
using GalaxyMerge.Client.UI.Connection.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace GalaxyMerge.Client.UI.Connection
{
    public class ConnectionModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ConnectionView, ConnectionViewModel>(ViewName.ConnectionView);
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}