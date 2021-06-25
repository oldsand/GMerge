using System;
using GalaxyMerge.Client.Core.Prism;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Core.Mvvm
{
    public class DialogViewModelBase : ViewModelBase, IDialogAware, IRegionManagerAware
    {
        private DelegateCommand _cancelDialogCommand;
        public DelegateCommand CancelDialogCommand =>
            _cancelDialogCommand ??= new DelegateCommand(ExecuteCancelDialog);
        
        public IRegionManager RegionManager { get; set; }

        public event Action<IDialogResult> RequestClose;


        protected virtual void ExecuteCancelDialog()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        protected virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
            
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}