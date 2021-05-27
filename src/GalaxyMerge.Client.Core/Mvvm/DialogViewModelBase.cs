using System;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Core.Mvvm
{
    public class DialogViewModelBase : ViewModelBase, IDialogAware
    {
        private DelegateCommand _closeDialogCommand;

        public DelegateCommand CloseDialogCommand =>
            _closeDialogCommand ??= new DelegateCommand(ExecuteCloseDialog);

        public event Action<IDialogResult> RequestClose;

        protected virtual void ExecuteCloseDialog()
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