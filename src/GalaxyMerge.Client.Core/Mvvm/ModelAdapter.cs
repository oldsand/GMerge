using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GalaxyMerge.Client.Core.Mvvm
{
    public class ModelAdapter<T> : Observable, IRevertibleChangeTracking
    {
        private readonly Dictionary<string, object> _originalValues;
        private readonly List<IRevertibleChangeTracking> _trackingObjects;

        protected ModelAdapter(T model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            _originalValues = new Dictionary<string, object>();
            _trackingObjects = new List<IRevertibleChangeTracking>();
        }

        public T Model { get; }

        public bool IsChanged => _originalValues.Count > 0 || 
                                 _trackingObjects.Any(t => t.IsChanged);

        public void AcceptChanges()
        {
            _originalValues.Clear();
            foreach (var trackingObject in _trackingObjects)
                trackingObject.AcceptChanges();
            
            OnPropertyChanged($"");
        }

        public void RejectChanges()
        {
            foreach (var originalValue in _originalValues)
                typeof(T).GetProperty(originalValue.Key)?.SetValue(Model, originalValue.Value);
            
            _originalValues.Clear();
            
            foreach (var trackingObject in _trackingObjects)
                trackingObject.RejectChanges();
            
            OnPropertyChanged($"");
        }

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            ValidatePropertyName(propertyName);
            var propertyInfo = Model.GetType().GetProperty(propertyName!);
            return (TValue) propertyInfo?.GetValue(Model);
        }

        protected TValue GetOriginalValue<TValue>(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName)
                ? (TValue) _originalValues[propertyName]
                : GetValue<TValue>(propertyName);
        }

        protected bool GetIsChanged(string propertyName)
        {
            return _originalValues.ContainsKey(propertyName);
        }

        protected void SetValue<TValue>(TValue newValue, [CallerMemberName] string propertyName = null)
        {
            ValidatePropertyName(propertyName);
            var propertyInfo = Model.GetType().GetProperty(propertyName!);
            var currentValue = propertyInfo?.GetValue(Model);
            if (Equals(currentValue, newValue)) return;
            UpdateOriginalValue(currentValue, newValue, propertyName);
            propertyInfo?.SetValue(Model, newValue);
            OnPropertyChanged(propertyName);
            OnPropertyChanged(propertyName + "IsChanged");
        }

        private void UpdateOriginalValue(object currentValue, object newValue, string propertyName)
        {
            if (!_originalValues.ContainsKey(propertyName))
            {
                _originalValues.Add(propertyName, currentValue);
                OnPropertyChanged(nameof(IsChanged));
            }
            else
            {
                if (!Equals(_originalValues[propertyName], newValue)) return;
                _originalValues.Remove(propertyName);
                OnPropertyChanged(nameof(IsChanged));
            }
        }

        protected void RegisterCollection<TWrapper, TModel>(ObservableCollection<TWrapper> wrapperCollection,
            List<TModel> modelCollection) where TWrapper : ModelAdapter<TModel>
        {
            wrapperCollection.CollectionChanged += (s, e) =>
            {
                if (e.OldItems != null)
                    foreach (var item in e.OldItems.Cast<TWrapper>())
                        modelCollection.Remove(item.Model);

                if (e.NewItems != null)
                    modelCollection.AddRange(e.NewItems.Cast<TWrapper>().Select(item => item.Model));
            };
        }

        protected void RegisterComplexProperty<TModel>(ModelAdapter<TModel> wrapper)
        {
            if (!_trackingObjects.Contains(wrapper))
            {
                _trackingObjects.Add(wrapper);
                wrapper.PropertyChanged += TrackingObjectPropertyChanged;
            }
        }

        private void TrackingObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsChanged))
            {
                OnPropertyChanged(nameof(IsChanged));
            }
        }

        private void ValidatePropertyName(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName), @"Method call passed null for argument");

            if (Model.GetType().GetProperty(propertyName) == null)
                throw new ArgumentNullException(nameof(propertyName), @"Property does not exist underlying model");
        }
    }
}