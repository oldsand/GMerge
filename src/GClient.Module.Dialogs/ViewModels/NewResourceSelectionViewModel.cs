using System;
using GClient.Core.Mvvm;
using GClient.Core.Naming;
using GClient.Data.Entities;
using NLog;
using Prism.Commands;
using Prism.Regions;

namespace GClient.Module.Dialogs.ViewModels
{
    public class NewResourceSelectionViewModel : NavigationViewModelBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private DelegateCommand<string> _resourceTypeSelectionCommand;

        public DelegateCommand<string> ResourceTypeSelectionCommand =>
            _resourceTypeSelectionCommand ??= new DelegateCommand<string>(ExecuteResourceTypeSelectionCommand);
        
        private void ExecuteResourceTypeSelectionCommand(string resourceTypeName)
        {
            Enum.TryParse(resourceTypeName, out ResourceType resourceType);
            var parameters = new NavigationParameters {{"resourceType", resourceType}};
            RegionManager.RequestNavigate(RegionName.ContentRegion, ViewName.NewResourceInfoView, parameters);
        }
    }
}