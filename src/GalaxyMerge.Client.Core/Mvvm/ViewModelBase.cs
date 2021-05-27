using System.Windows.Controls;
using Prism.Mvvm;
using Prism.Navigation;

namespace GalaxyMerge.Client.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        private string _title;
        private ControlTemplate _icon;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ControlTemplate Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public virtual void Destroy()
        {
        }
    }
}