using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers;
using Prism.Regions;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class ResourceSettingsGeneralViewModel : NavigationViewModelBase
    {
        private string _tabLabel = "General";
        private ResourceEntryWrapper _resourceEntryWrapper;

        public string TabLabel
        {
            get => _tabLabel;
            set => SetProperty(ref _tabLabel, value);
        }

        public ResourceEntryWrapper ResourceEntryWrapper
        {
            get => _resourceEntryWrapper;
            set => SetProperty(ref _resourceEntryWrapper, value);
        }
        

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var resource = navigationContext.Parameters.GetValue<ResourceEntry>("resource");
            ResourceEntryWrapper = new ResourceEntryWrapper(resource);
        }
    }
}