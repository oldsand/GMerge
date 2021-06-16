using GalaxyMerge.Client.Core.Mvvm;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class ResourceSettingsOptionsViewModel : ViewModelBase
    {
        private string _tabLabel = "Options";

        public string TabLabel
        {
            get => _tabLabel;
            set => SetProperty(ref _tabLabel, value);
        }
    }
}