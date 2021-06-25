using Prism.Commands;

namespace GalaxyMerge.Client.Dialogs.Commands
{
    public interface IDialogCommands
    {
        public CompositeCommand ApplyCommand { get; }
        public CompositeCommand SaveCommand { get; }
        public CompositeCommand BackCommand { get; }
        public CompositeCommand OkCommand { get; }
        public CompositeCommand CancelCommand { get; }
        public CompositeCommand YesCommand { get; }
        public CompositeCommand NoCommand { get; }
    }
}