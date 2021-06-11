using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Repositories;
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
            containerRegistry.RegisterDialog<NewResourceView, NewResourceViewModel>(DialogName.NewResourceDialog);
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}