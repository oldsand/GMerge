using GClient.Core.Mvvm;
using GClient.Core.Naming;
using GClient.Events;
using NLog;
using Prism.Events;
using Prism.Services.Dialogs;

namespace GClient.Module.Dialogs.ViewModels
{
    public sealed class NewResourceDialogModel : DialogViewModelBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public NewResourceDialogModel(IEventAggregator eventAggregator)
        {
            Logger.Trace("Initializing New Resource Dialog");
            Title = "New Resource";
            eventAggregator.GetEvent<NewResourceCompleteEvent>().Subscribe(OnNewResourceComplete);
        }

        private void OnNewResourceComplete(NewResourceCompleteEventArgs args)
        {
            var parameters = new DialogParameters {{"resource", args.ResourceEntry}};
            var result = new DialogResult(args.Result, parameters);
            RaiseRequestClose(result);
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Logger.Trace("Calling navigation requrest to the resrouce selection view");
            RegionManager.RequestNavigate(RegionName.ContentRegion, ViewName.NewResourceSelectionView);
        }
    }
}