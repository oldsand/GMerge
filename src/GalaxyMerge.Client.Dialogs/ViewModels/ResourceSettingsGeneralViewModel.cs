using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Observables;
using Prism.Regions;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class ResourceSettingsGeneralViewModel : NavigationViewModelBase
    {
        private string _tabLabel = "General";
        private ObservableResourceEntry _observableResourceEntry;

        public string TabLabel
        {
            get => _tabLabel;
            set => SetProperty(ref _tabLabel, value);
        }

        public ObservableResourceEntry ObservableResourceEntry
        {
            get => _observableResourceEntry;
            set => SetProperty(ref _observableResourceEntry, value);
        }
        

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var resource = navigationContext.Parameters.GetValue<ResourceEntry>("resource");
            ObservableResourceEntry = new ObservableResourceEntry(resource);
        }
    }
}