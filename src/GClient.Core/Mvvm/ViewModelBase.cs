using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using Prism.Mvvm;
using Prism.Navigation;

namespace GClient.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {

        private string _title;
        private ControlTemplate _icon;
        private bool _loading;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }

        public ControlTemplate Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public virtual void Destroy()
        {
        }

        protected virtual void Load()
        {
            Loading = true;
        }
        
        protected virtual Task LoadAsync()
        {
            Loading = true;
            return Task.CompletedTask;
        }

        protected virtual void OnLoadError(Exception ex)
        {
            Loading = false;
        }
        
        protected virtual void OnLoadComplete()
        {
            Loading = false;
        }
    }
}