using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GalaxyMerge.Client.Observables.Base
{
    public abstract class ObservableModel<T> : Observable, IRevertibleChangeTracking
    {
        private readonly Dictionary<string, object> _originalValues;
        private readonly List<IRevertibleChangeTracking> _trackingObjects;

        protected ObservableModel(T model)
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

            RaisePropertyChanged($"");
        }

        public void RejectChanges()
        {
            foreach (var originalValue in _originalValues)
                typeof(T).GetProperty(originalValue.Key)?.SetValue(Model, originalValue.Value);

            _originalValues.Clear();

            foreach (var trackingObject in _trackingObjects)
                trackingObject.RejectChanges();

            RaisePropertyChanged($"");
        }

        public TValue GetOriginalValue<TValue>(Expression<Func<T, TValue>> propertyExpression)
        {
            var expression = (MemberExpression) propertyExpression.Body;
            var propertyName = expression.Member.Name;

            return _originalValues.ContainsKey(propertyName)
                ? (TValue) _originalValues[propertyName]
                : GetValue<TValue>(propertyName);
        }

        public bool GetIsChanged<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var expression = (MemberExpression) propertyExpression.Body;
            var propertyName = expression.Member.Name;

            return _originalValues.ContainsKey(propertyName);
        }

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            return (TValue) propertyInfo?.GetValue(Model);
        }

        protected void SetValue<TValue>(TValue newValue, [CallerMemberName] string propertyName = null)
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            var currentValue = propertyInfo?.GetValue(Model);

            if (Equals(currentValue, newValue)) return;

            UpdateOriginalValue(currentValue, newValue, propertyName);
            propertyInfo?.SetValue(Model, newValue);
            RaisePropertyChanged(propertyName);
            RaisePropertyChanged(propertyName + "IsChanged");
        }

        protected void RegisterTrackingObject<TTracking>(TTracking trackingObject)
            where TTracking : IRevertibleChangeTracking, INotifyPropertyChanged
        {
            if (_trackingObjects.Contains(trackingObject)) return;

            _trackingObjects.Add(trackingObject);

            trackingObject.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsChanged))
                    RaisePropertyChanged(nameof(IsChanged));
            };
        }

        protected void RegisterCollection<TObservable, TModel>(ObservableCollection<TObservable> observableCollection,
            ICollection<TModel> modelCollection) where TObservable : ObservableModel<TModel>
        {
            observableCollection.CollectionChanged += (s, e) =>
            {
                if (observableCollection.Count > 0)
                {
                    if (e.OldItems != null)
                        foreach (var item in e.OldItems.Cast<TObservable>())
                            modelCollection.Remove(item.Model);

                    if (e.NewItems == null) return;
                    
                    foreach (var item in e.NewItems.Cast<TObservable>())
                        modelCollection.Add(item.Model);
                }
                else
                {
                    modelCollection.Clear();    
                }
            };
        }

        protected void RegisterCollection<TObservable>(ObservableCollection<TObservable> collection,
            NotifyCollectionChangedEventHandler handler)
        {
            collection.CollectionChanged += handler;
        }

        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName), @"Property name can not be null");

            var propertyInfo = Model.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
                throw new InvalidOperationException("Could not retrieve property info on give model");

            return propertyInfo;
        }

        private void UpdateOriginalValue(object currentValue, object newValue, string propertyName)
        {
            if (!_originalValues.ContainsKey(propertyName))
            {
                _originalValues.Add(propertyName, currentValue);
                RaisePropertyChanged(nameof(IsChanged));
                return;
            }

            if (!Equals(_originalValues[propertyName], newValue)) return;
            _originalValues.Remove(propertyName);
            RaisePropertyChanged(nameof(IsChanged));
        }
    }
}