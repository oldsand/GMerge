using GalaxyMerge.Client.Dialogs.ViewModels;

namespace GalaxyMerge.Client.Dialogs.Design
{
    public class DesignModels
    {
        public static NewResourceDialogModel NewResourceDialogModel => new();
        public static NewResourceSelectionViewModel NewResourceSelectionViewModel => new();
        public static NewResourceInfoViewModel NewResourceInfoViewModel => new();
        public static ResourceSettingsViewModel ResourceSettingsViewModel => new();
        public static ResourceSettingsGeneralViewModel ResourceSettingsGeneralViewModel => new();
    }
}