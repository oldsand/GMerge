using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class ResourceSettingsDialogModel : DialogViewModelBase
    {
        private ResourceEntryWrapper _resourceEntry;

        public ResourceSettingsDialogModel()
        {
            Title = "Resource Settings";
        }
        
        public ResourceEntryWrapper ResourceEntry
        {
            get => _resourceEntry;
            set => SetProperty(ref _resourceEntry, value);
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ResourceEntry = parameters.GetValue<ResourceEntryWrapper>("resource");

            if (ResourceEntry == null) return;
            
            PopulateSettingsTabs();
        }

        private void PopulateSettingsTabs()
        {
            var parameters = new NavigationParameters {{"resource", ResourceEntry}};

            RegionManager.RegisterViewWithRegion(RegionName.TabRegion, ViewName.ResourceSettingsGeneralView);
            RegionManager.RequestNavigate(RegionName.TabRegion, ViewName.ResourceSettingsOptionsView, parameters);
            RegionManager.RequestNavigate(RegionName.TabRegion, ViewName.ResourceSettingsGeneralView, parameters);
        }
    }
}