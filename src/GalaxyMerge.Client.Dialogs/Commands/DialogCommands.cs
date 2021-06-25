using Prism.Commands;

namespace GalaxyMerge.Client.Dialogs.Commands
{
    public class DialogCommands : IDialogCommands
    {
        public CompositeCommand ApplyCommand { get; } = new();
        public CompositeCommand SaveCommand { get; } = new();
        public CompositeCommand BackCommand { get; } = new();
        public CompositeCommand OkCommand { get; } = new();
        public CompositeCommand CancelCommand { get; } = new();
        public CompositeCommand YesCommand { get; } = new();
        public CompositeCommand NoCommand { get; } = new();
    }
}