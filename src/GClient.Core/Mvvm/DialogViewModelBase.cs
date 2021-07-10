using System;
using GClient.Core.Prism;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace GClient.Core.Mvvm
{
    public class DialogViewModelBase : ViewModelBase, IDialogAware, IRegionManagerAware
    {
        private DelegateCommand _applyCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _okCommand;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _yesCommand;
        private DelegateCommand _noCommand;
        private DelegateCommand _retryCommand;
        private DelegateCommand _abortCommand;

        public virtual DelegateCommand ApplyCommand =>
            _applyCommand ??= new DelegateCommand(ExecuteApplyCommand, ExecuteCanApplyCommand);

        public virtual DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(ExecuteSaveCommand, ExecuteCanSaveCommand);

        public virtual DelegateCommand OkCommand =>
            _okCommand ??= new DelegateCommand(ExecuteOkCommand);

        public virtual DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(ExecuteCancelCommand);

        public virtual DelegateCommand YesCommand =>
            _yesCommand ??= new DelegateCommand(ExecuteYesCommand);

        public virtual DelegateCommand NoCommand =>
            _noCommand ??= new DelegateCommand(ExecuteNoCommand);

        public virtual DelegateCommand RetryCommand =>
            _retryCommand ??= new DelegateCommand(ExecuteRetryCommand);

        public DelegateCommand AbortCommand =>
            _abortCommand ??= new DelegateCommand(ExecuteAbortCommand);

        public IRegionManager RegionManager { get; set; }

        public event Action<IDialogResult> RequestClose;

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

        protected virtual void ExecuteApplyCommand()
        {
            throw new NotImplementedException();
        }

        protected virtual bool ExecuteCanApplyCommand()
        {
            return false;
        }

        protected virtual void ExecuteSaveCommand()
        {
            throw new NotImplementedException();
        }

        protected virtual bool ExecuteCanSaveCommand()
        {
            return false;
        }

        protected virtual void ExecuteOkCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        protected virtual void ExecuteCancelCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }

        protected virtual void ExecuteYesCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Yes));
        }

        protected virtual void ExecuteNoCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.No));
        }

        protected virtual void ExecuteRetryCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Retry));
        }

        protected virtual void ExecuteAbortCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Abort));
        }
    }
}