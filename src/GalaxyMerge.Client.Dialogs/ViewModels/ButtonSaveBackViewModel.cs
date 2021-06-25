using GalaxyMerge.Client.Core.Mvvm;
using GalaxyMerge.Client.Dialogs.Commands;

namespace GalaxyMerge.Client.Dialogs.ViewModels
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