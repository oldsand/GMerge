using System;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class ResourceSettingsViewModel : DialogViewModelBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private ResourceEntryWrapper _resourceEntry;

        public ResourceSettingsViewModel()
        {
        }
        
        public ResourceSettingsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public ResourceEntryWrapper ResourceEntry
        {
            get => _resourceEntry;
            set => SetProperty(ref _resourceEntry, value);
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ResourceEntry = parameters.GetValue<ResourceEntryWrapper>("resource");

            LoadResourceSettingTabs();
        }

        private void LoadResourceSettingTabs()
        {
            var parameters = new NavigationParameters {{"resource", ResourceEntry}};

            /*switch (ResourceEntry.ResourceType)
            {
                case ResourceType.None:
                    break;
                case ResourceType.Connection:
                    _regionManager.RequestNavigate(RegionName.ResourceSettingsTabRegion,
                        ViewName.ResourceSettingsGeneralView, parameters);
                    _regionManager.RequestNavigate(RegionName.ResourceSettingsTabRegion,
                        ViewName.ResourceSettingsOptionsView, parameters);
                    break;
                case ResourceType.Archive:
                    break;
                case ResourceType.Directory:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }*/
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}