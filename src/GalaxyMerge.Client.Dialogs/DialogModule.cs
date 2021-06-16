using GalaxyMerge.Client.Core.Naming;
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
            containerRegistry.RegisterDialog<ConfirmationView, ConfirmationViewModel>(DialogName.ConfirmationDialog);
            containerRegistry.RegisterDialog<NewResourceView, NewResourceViewModel>(DialogName.NewResourceDialog);
            containerRegistry.RegisterDialog<ResourceSettingsView, ResourceSettingsViewModel>(DialogName
                .ResourceSettingsDialog);

            containerRegistry.RegisterForNavigation<ResourceSettingsView, ResourceSettingsViewModel>(
                ViewName.ResourceSettingsView);
            containerRegistry.RegisterForNavigation<ResourceSettingsGeneralView, ResourceSettingsGeneralViewModel>(
                ViewName.ResourceSettingsGeneralView);
            containerRegistry.RegisterForNavigation<ResourceSettingsOptionsView, ResourceSettingsOptionsViewModel>(
                ViewName.ResourceSettingsOptionsView);
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }
    }
}