using System;
using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers;
using Prism.Regions;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class ResourceSettingsGeneralViewModel : NavigationViewModelBase
    {
        
        private ResourceEntryWrapper _resourceEntry;

        public ResourceEntryWrapper ResourceEntry
        {
            get => _resourceEntry;
            set => SetProperty(ref _resourceEntry, value);
        }
        

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = "General";
            
            var resource = navigationContext.Parameters.GetValue<ResourceEntry>("resource");
            if (resource == null)
                throw new ArgumentNullException(nameof(resource), @"ResourceEntryWrapper cannot be null");
            
            ResourceEntry = new ResourceEntryWrapper(resource);
        }
        
        
    }
}