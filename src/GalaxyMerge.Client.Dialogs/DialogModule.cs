using GalaxyMerge.Client.Dialogs.ViewModels;
using GalaxyMerge.Client.Dialogs.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace GalaxyMerge.Client.Dialogs
{
    public class DialogModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<AddResourceView, AddResourceViewModel>(DialogName.AddResourceDialog);
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}