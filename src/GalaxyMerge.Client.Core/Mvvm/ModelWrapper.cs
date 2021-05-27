using System;
using Prism.Mvvm;

namespace GalaxyMerge.Client.Core.Mvvm
{
    public class ModelWrapper<T> : BindableBase where T : class
    {
        public ModelWrapper(T model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
        
        public T Model { get; }

        private string _propertyName;

        public string PropertyName
        {
            get => _propertyName;
            set => SetProperty(ref _propertyName, value);
        }
    }
}