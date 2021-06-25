using System;
using GalaxyMerge.Client.Core.Prism;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Core.Mvvm
{
    public class DialogViewModelBase : ViewModelBase, IDialogAware, IRegionManagerAware
    {
        private DelegateCommand _applyCommand;
        private DelegateCommand _saveCommand;
        private DelegateCommand _okCommand;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _yesCommand;
        private DelegateCommand _noCommand;

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

        public virtual void ExecuteApplyCommand()
        {
            throw new NotImplementedException();
        }

        public virtual bool ExecuteCanApplyCommand()
        {
            return false;
        }

        public virtual void ExecuteSaveCommand()
        {
            throw new NotImplementedException();
        }

        public virtual bool ExecuteCanSaveCommand()
        {
            return false;
        }
        
        public virtual void ExecuteOkCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        public virtual void ExecuteCancelCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }
        
        public virtual void ExecuteYesCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Yes));
        }
        
        public virtual void ExecuteNoCommand()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.No));
        }

    }
}