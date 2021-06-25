using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Naming;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Core;
using NLog;
using Prism.Commands;
using Prism.Regions;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class NewResourceSelectionViewModel : NavigationViewModelBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private DelegateCommand<string> _resourceTypeSelectionCommand;

        public DelegateCommand<string> ResourceTypeSelectionCommand =>
            _resourceTypeSelectionCommand ??= new DelegateCommand<string>(ExecuteResourceTypeSelectionCommand);
        
        private void ExecuteResourceTypeSelectionCommand(string resourceTypeName)
        {
            var resourceType = Enumeration.FromName<ResourceType>(resourceTypeName);
            var parameters = new NavigationParameters {{"resourceType", resourceType}};
            RegionManager.RequestNavigate(RegionName.ContentRegion, ViewName.NewResourceInfoView, parameters);
        }
    }
}