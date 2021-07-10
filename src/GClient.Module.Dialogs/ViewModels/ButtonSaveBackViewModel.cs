using GClient.Core.Mvvm;
using GClient.Module.Dialogs.Commands;

namespace GClient.Module.Dialogs.ViewModels
{
    public class ButtonSaveBackViewModel : ViewModelBase
    {
        private IDialogCommands _dialogCommands;

        public ButtonSaveBackViewModel(IDialogCommands dialogCommands)
        {
            DialogCommands = dialogCommands;
        }

        public IDialogCommands DialogCommands
        {
            get => _dialogCommands;
            private set => SetProperty(ref _dialogCommands, value);
        }
    }
}