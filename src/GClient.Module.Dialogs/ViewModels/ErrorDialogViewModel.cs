using System;
using System.Windows;
using GClient.Core.Mvvm;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace GClient.Module.Dialogs.ViewModels
{
    public class ErrorDialogViewModel : DialogViewModelBase
    {
        private string _message;
        private Exception _exception;
        private string _errorMessage;
        private string _stackTrace;
        
        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        public string StackTrace
        {
            get => _stackTrace;
            set => SetProperty(ref _stackTrace, value);
        }

        private DelegateCommand _copyDetailsCommand;

        public DelegateCommand CopyDetailsCommand =>
            _copyDetailsCommand ??= new DelegateCommand(ExecuteCopyDetailsCommand);

        private void ExecuteCopyDetailsCommand()
        {
            Clipboard.SetText(ErrorMessage);
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            Message = parameters.GetValue<string>("message");
            _exception = parameters.GetValue<Exception>("exception");
            
            ErrorMessage = _exception.Message;
            StackTrace = _exception.StackTrace;
        }
    }
}