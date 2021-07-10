using GClient.Core.Naming;
using GClient.Module.Dialogs.Commands;
using GClient.Module.Dialogs.ViewModels;
using GClient.Module.Dialogs.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GClient.Module.Dialogs
{
    public class DialogModule : IModule
    {
        
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDialogCommands, DialogCommands>();
        
            containerRegistry.RegisterDialog<ConfirmationView, ConfirmationViewModel>(DialogName.ConfirmationDialog);
            containerRegistry.RegisterDialog<ErrorDialogView, ErrorDialogViewModel>(DialogName.ErrorDialog);
            containerRegistry.RegisterDialog<NewResourceDialog, NewResourceDialogModel>(DialogName.NewResourceDialog);
            containerRegistry.RegisterDialog<ResourceSettingsDialog, ResourceSettingsDialogModel>(DialogName
                .ResourceSettingsDialog);


            containerRegistry.RegisterForNavigation<NewResourceSelectionView, NewResourceSelectionViewModel>(
                ViewName.NewResourceSelectionView);
            containerRegistry.RegisterForNavigation<NewResourceInfoView, NewResourceInfoViewModel>(
                ViewName.NewResourceInfoView);
            containerRegistry.RegisterForNavigation<ResourceSettingsDialog, ResourceSettingsDialogModel>(
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