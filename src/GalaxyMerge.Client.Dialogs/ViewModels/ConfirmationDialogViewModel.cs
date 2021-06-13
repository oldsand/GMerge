using GalaxyMerge.Client.Core.Mvvm;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Dialogs.ViewModels
{
    public class ConfirmationDialogViewModel : DialogViewModelBase
    {
        private string _message;
        private DelegateCommand _confirmCommand;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
        }

        public DelegateCommand ConfirmCommand =>
            _confirmCommand ??= new DelegateCommand(ExecuteConfirmCommand);

        private void ExecuteConfirmCommand()
        {
            var result = new DialogResult(ButtonResult.OK);
            RaiseRequestClose(result);
        }
    }
}