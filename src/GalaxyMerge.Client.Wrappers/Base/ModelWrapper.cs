﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GalaxyMerge.Client.Wrappers.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ModelWrapper<T> : ObservableErrorInfo, IValidatableChangeTracking, IValidatableObject
    {
        private readonly Dictionary<string, object> _originalValues;
        private readonly Dictionary<string, IValidatableChangeTracking> _trackingObjects;

        /// <summary>
        /// Base constructor for the ModelWrapper. This constructor optionally performs registration by making
        /// a call to the overridable Register method. By default the Register method uses reflection to attempt
        /// registering any IValidatableChangeTracking objects with the parent model so that the root IsChanged property can
        /// be kept in sync with child model object changes. Consumers should override Register in models that contain
        /// complex properties or collections to perform initialization prior to registration.
        /// Consumers may also manually call RegisterChangeTracking to bypass the default reflection implementation.
        /// Any collection members can also call RegisterCollection in the overridable Register method.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="callRegister"></param>
        /// <exception cref="ArgumentNullException">Thrown when the model property is null</exception>
        protected ModelWrapper(T model, bool callRegister = false)
        {
            Model = model;
            _originalValues = new Dictionary<string, object>();
            _trackingObjects = new Dictionary<string, IValidatableChangeTracking>();

            if (callRegister)
                RunRegistration(model);
        }

        public T Model { get; }

        public bool IsChanged =>
            _originalValues.Count > 0 || _trackingObjects.Select(t => t.Value).Any(t => t.IsChanged);

        public bool IsValid => !HasErrors && _trackingObjects.Select(t => t.Value).All(t => t.IsValid);

        public void AcceptChanges()
        {
            _originalValues.Clear();

            foreach (var trackingObject in _trackingObjects.Values)
                trackingObject.AcceptChanges();

            RaisePropertyChanged($"");
        }

        public void RejectChanges()
        {
            foreach (var originalValue in _originalValues)
                typeof(T).GetProperty(originalValue.Key)?.SetValue(Model, originalValue.Value);

            _originalValues.Clear();

            foreach (var trackingObject in _trackingObjects.Values)
                trackingObject.RejectChanges();

            ValidateObject();
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

        public IEnumerable<string> GetErrors(Expression<Func<T, object>> propertyExpression)
        {
            var expressionBody = (MemberExpression) propertyExpression.Body;
            var propertyName = expressionBody.Member.Name;

            return GetErrors(propertyName).Cast<string>();
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }

        protected void RunValidation()
        {
            ValidateObject();
        }
        
        protected void RunValidation(string propertyName)
        {
            ValidateProperty(propertyName);
        }

        protected virtual void Register(T model)
        {
            var properties = GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(this);
                if (value is IValidatableChangeTracking tracking)
                    RegisterTrackingObjectInternal(property.Name, tracking);
            }
        }

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            return (TValue) propertyInfo?.GetValue(Model);
        }

        protected void SetValue<TValue>(TValue newValue, Action<T, TValue> setValue = null, Action onChanged = null,
            [CallerMemberName] string propertyName = null)
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            var currentValue = propertyInfo?.GetValue(Model);

            if (newValue.Equals(currentValue)) return;

            UpdateOriginalValue(currentValue, newValue, propertyName);

            if (setValue != null)
                setValue.Invoke(Model, newValue);
            else
                propertyInfo?.SetValue(Model, newValue);
            
            ValidateProperty(propertyName);

            onChanged?.Invoke();

            RaisePropertyChanged(propertyName);
            RaisePropertyChanged(propertyName + "IsChanged");
        }

        protected void SetValue<TWrapper, TModel>(TWrapper newValue, 
            Action<T, TModel> setValue = null,
            Action onChanged = null,
            Func<TModel, TModel, bool> comparer = null,
            bool autoRegister = true,
            [CallerMemberName] string propertyName = null)
            where TWrapper : ModelWrapper<TModel>
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            var currentValue = propertyInfo?.GetValue(Model);

            if (AreEqual(newValue, (TModel) currentValue, comparer)) return;

            UpdateOriginalValue((TModel) currentValue, newValue, propertyName);

            if (autoRegister)
                RegisterTrackingObjectInternal(propertyName, newValue);

            if (setValue != null)
                setValue.Invoke(Model, newValue.Model);
            else
                propertyInfo?.SetValue(Model, newValue.Model);

            ValidateProperty(propertyName);
            
            onChanged?.Invoke();

            RaisePropertyChanged(propertyName);
            RaisePropertyChanged(propertyName + "IsChanged");
        }

        protected void RegisterTrackingObject(string propertyName, IValidatableChangeTracking trackingObject)
        {
            RegisterTrackingObjectInternal(propertyName, trackingObject);
        }

        protected void RegisterCollection<TWrapper, TModel>(ObservableCollection<TWrapper> observableCollection,
            ICollection<TModel> modelCollection) where TWrapper : ModelWrapper<TModel>
        {
            observableCollection.CollectionChanged += (_, e) =>
            {
                if (observableCollection.Count > 0)
                {
                    if (e.OldItems != null)
                        foreach (var item in e.OldItems.Cast<TWrapper>())
                            modelCollection.Remove(item.Model);

                    if (e.NewItems == null) return;

                    foreach (var item in e.NewItems.Cast<TWrapper>())
                        modelCollection.Add(item.Model);
                }
                else
                {
                    modelCollection.Clear();
                }

                ValidateObject();
            };
        }

        protected void RegisterCollection<TObservable>(ObservableCollection<TObservable> collection,
            NotifyCollectionChangedEventHandler changedHandler)
        {
            collection.CollectionChanged += changedHandler;
            collection.CollectionChanged += (_, _) => ValidateObject();
        }

        private void ValidateObject()
        {
            ClearErrors();

            var results = new List<ValidationResult>();
            var context = new ValidationContext(this);
            Validator.TryValidateObject(this, context, results, true);
            
            if (results.Any())
            {
                var propertyNames = results.SelectMany(r => r.MemberNames).Distinct().ToList();
                foreach (var propertyName in propertyNames)
                {
                    Errors[propertyName] = results
                        .Where(r => r.MemberNames.Contains(propertyName))
                        .Select(r => r.ErrorMessage)
                        .Distinct()
                        .ToList();

                    RaiseErrorsChanged(propertyName);
                }
            }

            RaisePropertyChanged(nameof(IsValid));
        }
        
        private void ValidateProperty(string propertyName)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName};
            Validator.TryValidateObject(this, context, results, true);
            //Validator.TryValidateProperty(newValue, context, results);
            
            var propertyErrors = results.Where(r => r.MemberNames.Contains(propertyName)).ToList();

            if (propertyErrors.Any())
            {
                Errors[propertyName] = propertyErrors.Select(r => r.ErrorMessage).Distinct().ToList();
                RaiseErrorsChanged(propertyName);
                RaisePropertyChanged(nameof(IsValid));
            }
            else if (Errors.ContainsKey(propertyName))
            {
                Errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
                RaisePropertyChanged(nameof(IsValid));
            }
        }

        private void RegisterTrackingObjectInternal(string propertyName, IValidatableChangeTracking trackingObject)
        {
            if (_trackingObjects.ContainsKey(propertyName))
            {
                var current = _trackingObjects[propertyName];
                if (ReferenceEquals(current, trackingObject)) return;
                current.PropertyChanged -= TrackingObjectOnPropertyChanged;
                _trackingObjects.Remove(propertyName);
            }

            _trackingObjects.Add(propertyName, trackingObject);
            trackingObject.PropertyChanged += TrackingObjectOnPropertyChanged;
        }

        private void TrackingObjectOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(IsChanged) or nameof(IsValid))
                RaisePropertyChanged(e.PropertyName);
        }

        private void UpdateOriginalValue(object currentValue, object newValue, string propertyName)
        {
            if (!_originalValues.ContainsKey(propertyName))
            {
                _originalValues.Add(propertyName, currentValue);
                RaisePropertyChanged(nameof(IsChanged));
                return;
            }

            if (!newValue.Equals(_originalValues[propertyName])) return;
            _originalValues.Remove(propertyName);
            RaisePropertyChanged(nameof(IsChanged));
        }

        private void UpdateOriginalValue<TWrapper, TModel>(TModel currentValue, TWrapper newValue, string propertyName,
            Func<TModel, TModel, bool> comparer = null)
            where TWrapper : ModelWrapper<TModel>
        {
            if (!_originalValues.ContainsKey(propertyName))
            {
                _originalValues.Add(propertyName, currentValue);
                RaisePropertyChanged(nameof(IsChanged));
                return;
            }

            if (!AreEqual(newValue, (TModel) _originalValues[propertyName], comparer)) return;
            _originalValues.Remove(propertyName);
            RaisePropertyChanged(nameof(IsChanged));
        }

        private static bool AreEqual<TWrapper, TModel>(TWrapper newValue, TModel currentValue,
            Func<TModel, TModel, bool> comparer = null) where TWrapper : ModelWrapper<TModel>
        {
            if (comparer != null)
                return comparer.Invoke(currentValue, newValue.Model);

            // ReSharper disable once SuspiciousTypeConversion.Global because maybe there are no implementations?
            if (newValue is IEquatable<TModel> equatable)
                return equatable.Equals(currentValue);

            if (ReferenceEquals(newValue.Model, currentValue)) return true;
            if (ReferenceEquals(newValue.Model, null) && ReferenceEquals(currentValue, null)) return true;
            if (ReferenceEquals(newValue.Model, null) || ReferenceEquals(currentValue, null)) return false;
            return currentValue.GetType() == newValue.Model.GetType() && PropertiesEquate(newValue, currentValue);
        }

        private static bool PropertiesEquate<TWrapper, TModel>(TWrapper newValue, TModel currentValue)
            where TWrapper : ModelWrapper<TModel>
        {
            var currentProperties = currentValue.GetType().GetProperties();

            return !(from property in currentProperties
                let c = property.GetValue(currentValue)
                let n = property.GetValue(newValue.Model)
                where !Equals(c, n)
                select c).Any();
        }

        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName), "Property name can not be null");

            /*if (Model == null)
                throw new InvalidOperationException("Model object is null. Cannot get property information");*/

            var propertyInfo = Model.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Could not retrieve property info for {propertyName}");

            return propertyInfo;
        }

        private void RunRegistration(T model)
        {
            Register(model);
        }
    }
}