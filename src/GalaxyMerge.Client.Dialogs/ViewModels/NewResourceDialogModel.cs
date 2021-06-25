using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Core.Naming;
using NLog;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public sealed class NewResourceDialogModel : DialogViewModelBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public NewResourceDialogModel()
        {
            Logger.Trace("Initializing New Resource Dialog");
            Title = "New Resource";
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Logger.Trace("Calling navigation requrest to the resrouce selection view");
            RegionManager.RequestNavigate(RegionName.ContentRegion, ViewName.NewResourceSelectionView);
        }
    }
}