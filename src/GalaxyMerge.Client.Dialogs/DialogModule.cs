using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.Dialogs.ViewModels;
using GalaxyMerge.Client.Dialogs.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GalaxyMerge.Client.Dialogs
{
    public class DialogModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public DialogModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<ConfirmationView, ConfirmationViewModel>(DialogName.ConfirmationDialog);
            containerRegistry.RegisterDialog<NewResourceDialog, NewResourceDialogModel>(DialogName.NewResourceDialog);
            containerRegistry.RegisterDialog<ResourceSettingsView, ResourceSettingsViewModel>(DialogName
                .ResourceSettingsDialog);


            containerRegistry.RegisterForNavigation<NewResourceSelectionView, NewResourceSelectionViewModel>(
                ViewName.NewResourceSelectionView);
            containerRegistry.RegisterForNavigation<NewResourceInfoView, NewResourceInfoViewModel>(
                ViewName.NewResourceInfoView);
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