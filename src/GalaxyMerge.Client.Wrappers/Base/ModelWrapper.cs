using System;
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
    public abstract class ModelWrapper<T> : ObservableErrorInfo, ITrackingObject, IValidatableObject
    {
        private readonly Dictionary<string, object> _originalValues = new();
        private readonly Dictionary<string, ITrackingObject> _trackingObjects = new();
        private readonly List<string> _requiredProperties = new();
        private readonly Dictionary<string, RequiredAttribute> _requiredAttributes = new();

        /// <summary>
        /// Base constructor for the ModelWrapper. This constructor optionally performs initialization by making
        /// a call to the virtual Initialize method. The base initialize method uses reflection to attempt to register
        /// any ITrackingObjects members and update required properties that have the RequiredAttribute data annotation.
        /// This can be disabled by setting callInitialize to false. Since Initialize would be called before the deriving
        /// class constructor, any member initialization should be done in Initialize by overriding the method.
        /// Otherwise null member references will not be automatically register for change tracking.
        /// </summary>
        /// <param name="model">The model instance to wrap</param>
        /// <param name="callInitialize">Calls virtual Initialize method on construction of base class</param>
        protected ModelWrapper(T model, bool callInitialize = true)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model), "Model cannot be null");

            if (callInitialize)
                RunInitialization(model);
        }

        public T Model { get; }

        public bool IsChanged => _originalValues.Count > 0 ||
                                 _trackingObjects.Select(t => t.Value).Any(t => t.IsChanged);

        public bool IsValid => !HasErrors && _trackingObjects.Select(t => t.Value).All(t => t.IsValid);

        public bool HasRequired => _requiredProperties.Any() ||
                                   _trackingObjects.Select(t => t.Value).Any(t => t.HasRequired);

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
            if (propertyExpression.Body is not MemberExpression memberExpression)
                throw new InvalidOperationException("");

            var propertyName = memberExpression.Member.Name;

            return _originalValues.ContainsKey(propertyName)
                ? (TValue) _originalValues[propertyName]
                : GetValue<TValue>(propertyName);
        }

        public bool GetIsChanged<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            if (propertyExpression.Body is not MemberExpression memberExpression)
                throw new InvalidOperationException("");

            var propertyName = memberExpression.Member.Name;

            return _originalValues.ContainsKey(propertyName);
        }

        public bool GetIsRequired<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            if (propertyExpression.Body is not MemberExpression memberExpression)
                throw new InvalidOperationException("");

            var propertyName = memberExpression.Member.Name;

            return _requiredProperties.Contains(propertyName);
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

        protected TValue GetValue<TValue>([CallerMemberName] string propertyName = null)
        {
            var propertyInfo = GetModelPropertyInfo(propertyName);
            return (TValue) propertyInfo?.GetValue(Model);
        }

        protected void SetValue<TValue>(TValue newValue, Action<T, TValue> setValue = null, Action onChanged = null,
            [CallerMemberName] string propertyName = null)
        {
            var propertyInfo = GetModelPropertyInfo(propertyName);
            var currentValue = propertyInfo?.GetValue(Model);

            if (Equals(currentValue, newValue)) return;

            UpdateOriginalValue(currentValue, newValue, propertyName);

            if (setValue != null)
                setValue.Invoke(Model, newValue);
            else
                propertyInfo?.SetValue(Model, newValue);
            
            UpdateRequiredProperty(propertyName, newValue);
            ValidateProperty(propertyName);

            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);
            RaisePropertyChanged(propertyName + "IsChanged");
        }

        protected void SetValue<TWrapper, TModel>(ref TWrapper storage, TWrapper newValue,
            Action<T, TModel> setValue = null,
            Action onChanged = null,
            Func<TModel, TModel, bool> comparer = null,
            bool autoRegister = true,
            [CallerMemberName] string propertyName = null)
            where TWrapper : ModelWrapper<TModel>
            where TModel : class
        {
            if (!EqualityComparer<TWrapper>.Default.Equals(storage, newValue))
                storage = newValue;
            
            var propertyInfo = GetModelPropertyInfo(propertyName);
            var currentModel = (TModel) propertyInfo?.GetValue(Model);
            var newModel = newValue?.Model;
            
            if (ReferenceEquals(currentModel, newModel)) return;
            
            if (!ModelEquals(currentModel, newModel, comparer))
                UpdateOriginalValue(currentModel, newModel, propertyName);
            
            if (autoRegister)
                RegisterTrackingObjectInternal(propertyName, newValue);
            
            if (setValue != null)
                setValue.Invoke(Model, newModel);
            else
                propertyInfo?.SetValue(Model, newModel);

            UpdateRequiredProperty(propertyName, newValue);
            ValidateProperty(propertyName);

            if (ModelEquals(currentModel, newModel, comparer)) return;
            
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);
            RaisePropertyChanged(propertyName + "IsChanged");
        }

        protected virtual void Initialize(T model)
        {
            var properties = GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(this);
                
                if (value is ITrackingObject tracking)
                    RegisterTrackingObjectInternal(property.Name, tracking);

                var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
                if (requiredAttribute != null && !_requiredAttributes.ContainsKey(property.Name))
                    RequireProperty(property.Name, requiredAttribute);
            
                UpdateRequiredProperty(property.Name, property.GetValue(this));
            }
        }

        protected void RegisterTrackingObject(string propertyName, ITrackingObject trackingObject)
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

        protected void RequireProperty(string propertyName, RequiredAttribute requiredAttribute = null)
        {
            requiredAttribute ??= new RequiredAttribute();
            
            if (_requiredAttributes.ContainsKey(propertyName))
            {
                var current = _requiredAttributes[propertyName];
                if (ReferenceEquals(current, requiredAttribute)) return;
                _requiredAttributes.Remove(propertyName);
            }

            _requiredAttributes.Add(propertyName, requiredAttribute);
        }

        protected void RunValidation()
        {
            ValidateObject();
        }

        protected void RunValidation(string propertyName)
        {
            ValidateProperty(propertyName);
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
            var context = new ValidationContext(this) {MemberName = propertyName};
            Validator.TryValidateObject(this, context, results, true);

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

        private void RegisterTrackingObjectInternal(string propertyName, ITrackingObject trackingObject)
        {
            if (_trackingObjects.ContainsKey(propertyName))
            {
                var current = _trackingObjects[propertyName];
                if (ReferenceEquals(current, trackingObject)) return;
                current.PropertyChanged -= TrackingObjectOnPropertyChanged;
                _trackingObjects.Remove(propertyName);
            }

            if (trackingObject == null) return;
            _trackingObjects.Add(propertyName, trackingObject);
            trackingObject.PropertyChanged += TrackingObjectOnPropertyChanged;
        }

        private void TrackingObjectOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(IsChanged) or nameof(IsValid) or nameof(HasRequired))
                RaisePropertyChanged(e.PropertyName);
        }

        private void RegisterRequired(PropertyInfo property)
        {
            if (_requiredAttributes.ContainsKey(property.Name))
            {
             
                return;
            }
            
            var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttribute != null)
                RequireProperty(property.Name, requiredAttribute);
            
            UpdateRequiredProperty(property.Name, property.GetValue(this));
        }

        private void UpdateRequiredProperty(string propertyName, object value)
        {
            if (!_requiredAttributes.ContainsKey(propertyName)) return;

            var required = _requiredAttributes[propertyName];

            if (required.IsValid(value))
            {
                if (!_requiredProperties.Contains(propertyName)) return;
                _requiredProperties.Remove(propertyName);
                RaisePropertyChanged(nameof(HasRequired));
                return;
            }

            if (_requiredProperties.Contains(propertyName)) return;
            _requiredProperties.Add(propertyName);
            RaisePropertyChanged(nameof(HasRequired));
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

        private void UpdateOriginalValue<TModel>(TModel currentValue, TModel newValue, string propertyName,
            Func<TModel, TModel, bool> comparer = null)
            where TModel : class
        {
            if (!_originalValues.ContainsKey(propertyName))
            {
                _originalValues.Add(propertyName, currentValue);
                RaisePropertyChanged(nameof(IsChanged));
                return;
            }

            if (!ModelEquals(newValue, (TModel) _originalValues[propertyName], comparer)) return;
            _originalValues.Remove(propertyName);
            RaisePropertyChanged(nameof(IsChanged));
        }

        private static bool ModelEquals<TModel>(TModel a, TModel b, Func<TModel, TModel, bool> comparer = null)
            where TModel : class
        {
            if (comparer != null)
                return comparer.Invoke(a, b);

            // ReSharper disable once SuspiciousTypeConversion.Global because maybe there are no implementations?
            if (a is IEquatable<TModel> equatable)
                return equatable.Equals(b);
            
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return b.GetType() == a.GetType() && PropertyEquals(a, b);
        }
        
        private static bool PropertyEquals<TModel>(TModel a, TModel b)
            where TModel : class
        {
            var properties = b.GetType().GetProperties();

            return !(from property in properties
                let x = property.GetValue(b)
                let y = property.GetValue(a)
                where !Equals(x, y)
                select x).Any();
        }

        private PropertyInfo GetModelPropertyInfo(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName), "Property name can not be null");

            var propertyInfo = Model.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
                throw new InvalidOperationException($"Could not retrieve property info for {propertyName}");

            return propertyInfo;
        }

        private void RunInitialization(T model)
        {
            Initialize(model);
        }
    }
}