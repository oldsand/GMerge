using System;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Wrappers;
using Prism.Regions;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class ResourceSettingsOptionsViewModel : NavigationViewModelBase
    {
        private ResourceEntryWrapper _resourceEntry;

        public ResourceSettingsOptionsViewModel()
        {
            Title = "Options";
        }

        public ResourceEntryWrapper ResourceEntry
        {
            get => _resourceEntry;
            set => SetProperty(ref _resourceEntry, value);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var resource = navigationContext.Parameters.GetValue<ResourceEntryWrapper>("resource");
            ResourceEntry = resource ?? throw new ArgumentNullException(nameof(resource), @"ResourceEntryWrapper cannot be null");
        }
    }
}