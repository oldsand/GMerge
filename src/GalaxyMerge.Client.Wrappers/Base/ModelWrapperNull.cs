using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public abstract class ModelWrapperNull<T> : ObservableErrorInfo, IValidatableChangeTracking, IValidatableObject
    {
        private readonly Dictionary<string, object> _originalValues;
        private readonly List<IValidatableChangeTracking> _trackingObjects;

        /// <summary>
        /// Base constructor for the ObservableModel. This constructor optionally performs initialization by making
        /// a call to the virtual Initialize method, as well as optionally performs validation in order to check the
        /// state of the model prior to and property changes.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="initializeOnConstruction">Calls Initialize during construction of the base class</param>
        /// <param name="validateOnConstruction">Cals private Validate method which perform validation during construction of the base class</param>
        /// <exception cref="ArgumentNullException">Thrown when the model property is null</exception>
        protected ModelWrapperNull(T model, bool initializeOnConstruction = true, bool validateOnConstruction = true)
        {
            Model = model;
            _originalValues = new Dictionary<string, object>();
            _trackingObjects = new List<IValidatableChangeTracking>();

            if (initializeOnConstruction)
                InitializeInternal(model);

            if (validateOnConstruction)
                Validate();
        }

        public T Model { get; }

        public bool IsChanged => _originalValues.Count > 0 || _trackingObjects.Any(t => t.IsChanged);

        public bool IsValid => !HasErrors && _trackingObjects.All(t => t.IsValid);

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

            Validate();
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

        public virtual void Initialize(T model)
        {
        }

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            return (TValue) propertyInfo?.GetValue(Model);
        }

        protected void SetValue<TValue>(TValue newValue, Action<T, TValue> customSet = null, Action onChanged = null, [CallerMemberName] string propertyName = null)
        {
            var propertyInfo = GetPropertyInfo(propertyName);
            var currentValue = propertyInfo?.GetValue(Model);

            if (Equals(currentValue, newValue)) return;

            UpdateOriginalValue(currentValue, newValue, propertyName);

            if (customSet != null)
                customSet.Invoke(Model, newValue);
            else
                propertyInfo?.SetValue(Model, newValue);

            onChanged?.Invoke();

            Validate();

            RaisePropertyChanged(propertyName);
            RaisePropertyChanged(propertyName + "IsChanged");
        }
        
        protected void SetValue<TValue>(ref TValue current, TValue newValue, Action<T, TValue> customSet = null, 
            Action onChanged = null, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TValue>.Default.Equals(current, newValue)) return;
            
            UpdateOriginalValue(current, newValue, propertyName);

            if (customSet != null)
                customSet.Invoke(Model, newValue);
            else
                current = newValue;
            
            onChanged?.Invoke();

            Validate();

            RaisePropertyChanged(propertyName);
            RaisePropertyChanged(propertyName + "IsChanged");
        }

        protected void RegisterTrackingObject(IValidatableChangeTracking trackingObject)
        {
            if (_trackingObjects.Contains(trackingObject)) return;

            _trackingObjects.Add(trackingObject);

            trackingObject.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName is nameof(IsChanged) or nameof(IsValid))
                    RaisePropertyChanged(e.PropertyName);
            };
        }

        protected void RegisterCollection<TObservable, TModel>(ObservableCollection<TObservable> observableCollection,
            ICollection<TModel> modelCollection) where TObservable : ModelWrapper<TModel>
        {
            observableCollection.CollectionChanged += (_, e) =>
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

                Validate();
            };
        }

        protected void RegisterCollection<TObservable>(ObservableCollection<TObservable> collection,
            NotifyCollectionChangedEventHandler changedHandler)
        {
            collection.CollectionChanged += changedHandler;
            collection.CollectionChanged += (_, _) => Validate();
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

        private PropertyInfo GetPropertyInfo(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName), @"Property name can not be null");
            
            var propertyInfo = Model.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Could not retrieve property info for {propertyName}");

            return propertyInfo;
        }

        private void Validate()
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

        private void InitializeInternal(T model)
        {
            Initialize(model);
        }
    }
}