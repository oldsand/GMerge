using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Client.Observables.Tests.Model;
using NUnit.Framework;

namespace GalaxyMerge.Client.Observables.Tests
{
    [TestFixture]
    public class ObservableModelTests
    {
        private GenericModel _model;

        [SetUp]
        public void Setup()
        {
            _model = new GenericModel
            {
                Name = "Generic",
                Description = "Test observable model",
                Number = 1,
                Value = 2.1,
                DateTime = DateTime.Today,
                Type = SomeType.Type2,
                SubModel = new SubModel
                {
                    Id = 12,
                    Name = "GenericSubModel",
                    Value = 123.456f
                },
                SubModels = new List<SubModel>
                {
                    new SubModel {Id=1, Name = "Item1", Value = 1.1f},
                    new SubModel {Id=3, Name = "Item2", Value = 1.2f},
                    new SubModel {Id=2, Name = "Item3", Value = 1.3f}
                }
            };
        }

        [Test]
        public void Constructor_ValidModel_ReturnsNotNull()
        {
            var observable = new ObservableGenericModel(_model);
            
            Assert.NotNull(observable);
            Assert.NotNull(observable.Model);
        }
        
        [Test]
        public void Constructor_NullReference_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var observable = new ObservableGenericModel(null);
            });
        }
        
        [Test]
        public void Constructor_ValidModelWithComplexProperty_ReturnsNotNullComplexProperty()
        {
            var observable = new ObservableGenericModel(_model);
            Assert.NotNull(observable.SubModel);
            Assert.AreEqual(_model.SubModel, observable.SubModel.Model);
        }
        
        [Test]
        public void Constructor_ValidModelCollectionProperty_ReturnsNotNullComplexProperty()
        {
            var observable = new ObservableGenericModel(_model);
            
            Assert.NotNull(observable.SubModels);
            Assert.AreEqual(_model.SubModels.Count(), observable.SubModels.Count);
            Assert.True(_model.SubModels.All(s => observable.SubModels.Any(x => x.Model == s)));
        }
        
        [Test]
        public void Constructor_ValidModelWithNullComplexProperty_ThrowsArgumentNullException()
        {
            _model.SubModel = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                var observable = new ObservableGenericModel(_model);
            });
        }
        
        [Test]
        public void RegisterCollection_AddItemObservable_ShouldMatchModelCollection()
        {
            var observable = new ObservableGenericModel(_model);
            
            observable.SubModels.Add(new ObservableSubModel(new SubModel {Id = 4, Name = "New Item", Value = 1.4f}));
            
            Assert.AreEqual(observable.SubModels.Count, _model.SubModels.Count);
            Assert.True(_model.SubModels.All(s => observable.SubModels.Any(x => x.Model == s)));
        }
        
        [Test]
        public void RegisterCollection_RemoveItemObservable_ShouldMatchModelCollection()
        {
            var observable = new ObservableGenericModel(_model);
            var target = observable.SubModels.FirstOrDefault();
            observable.SubModels.Remove(target);
            
            Assert.AreEqual(_model.SubModels.Count, observable.SubModels.Count);
            Assert.True(_model.SubModels.All(s => observable.SubModels.Any(x => x.Model == s)));
        }
        
        [Test]
        public void RegisterCollection_ClearItemObservable_ShouldMatchModelCollection()
        {
            var observable = new ObservableGenericModel(_model);
            
            observable.SubModels.Clear();
            
            Assert.AreEqual(_model.SubModels.Count, observable.SubModels.Count);
            Assert.True(_model.SubModels.All(s => observable.SubModels.Any(x => x.Model == s)));
        }

        [Test]
        public void RegisterCollectionChangedHandler_AddOtherItem_ShouldUpdateModelCollection()
        {
            var observable = new ObservableGenericModel(_model);
            
            observable.OtherModels.Add(new ObservableSubModel(new SubModel {Id = 1, Name = "Other Model Item", Value = 11.42f}));
            
            Assert.AreEqual(observable.OtherModels.Count, _model.OtherModels.Count);
            Assert.True(_model.OtherModels.All(s => observable.OtherModels.Any(x => x.Model == s)));
        }
        
        [Test]
        public void Constructor_ValidModelWithNullCollectionProperty_ThrowsArgumentNullException()
        {
            _model.SubModels = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                var observable = new ObservableGenericModel(_model);
            });
        }

        [Test]
        public void GetValue_NameProperty_GetsExpectedValue()
        {
            var observable = new ObservableGenericModel(_model);
            Assert.AreEqual(_model.Name, observable.Name);
        }

        [Test]
        public void SetValue_NameProperty_SetsModelProperty()
        {
            var observable = new ObservableGenericModel(_model);
            observable.Name = "New Name";
            Assert.AreEqual("New Name", _model.Name);
        }

        [Test]
        public void RaisePropertyChanged_NewValue_RaisesExpectedEvents()
        {
            var observable = new ObservableGenericModel(_model);
            
            var changed = new List<string>();
            observable.PropertyChanged += (s, e) =>
            {
                changed.Add(e.PropertyName);
            };
            
            observable.Name = "New Name";
            Assert.AreEqual(changed.Count, 3);
            
            Assert.AreEqual(changed[0], nameof(observable.IsChanged));
            Assert.AreEqual(changed[1], nameof(observable.Name));
            Assert.AreEqual(changed[2], "NameIsChanged");
        }
        
        [Test]
        public void RaisePropertyChanged_SameValue_RaisesExpectedEvents()
        {
            var observable = new ObservableGenericModel(_model);
            
            var changed = new List<string>();
            observable.PropertyChanged += (s, e) =>
            {
                changed.Add(e.PropertyName);
            };
            
            observable.Name = "Generic";
            Assert.AreEqual(changed.Count, 0);
        }
        
        [Test]
        public void RaisePropertyChanged_ComplexPropertyChanged_ReturnsChanged()
        {
            var observable = new ObservableGenericModel(_model);
            
            var changed = new List<string>();
            observable.PropertyChanged += (s, e) =>
            {
                changed.Add(e.PropertyName);
            };
            
            observable.SubModel.Name = "New Name";
            Assert.AreEqual(1, changed.Count);
            Assert.AreEqual(changed[0], nameof(observable.IsChanged));
        }

        [Test]
        public void GetOriginalValue_NameNoChanges_ReturnsExpectedValue()
        {
            var observable = new ObservableGenericModel(_model);
            
            var original = observable.GetOriginalValue(m => m.Name);
            
            Assert.AreEqual("Generic", original);
        }

        [Test]
        public void GetOriginalValue_NameChanged_ShouldReturnOriginalValue()
        {
            var observable = new ObservableGenericModel(_model);
            var original = observable.GetOriginalValue(m => m.Name);
            Assert.AreEqual("Generic", original);

            observable.Name = "New Name";
            
            original = observable.GetOriginalValue(m => m.Name);
            Assert.AreEqual("Generic", original);
        }
        
        [Test]
        public void GetIsChanged_NameNoChanges_ReturnsFalse()
        {
            var observable = new ObservableGenericModel(_model);
            
            var changed = observable.GetIsChanged(m => m.Name);
            
            Assert.False(changed);
            Assert.False(observable.IsChanged);
        }
        
        [Test]
        public void GetIsChanged_NameChanged_ReturnsTrue()
        {
            var observable = new ObservableGenericModel(_model);
            
            var changed = observable.GetIsChanged(m => m.Name);
            Assert.False(changed);
            Assert.False(observable.IsChanged);

            observable.Name = "New Name";
            
            changed = observable.GetIsChanged(m => m.Name);
            Assert.True(changed);
            Assert.True(observable.IsChanged);
        }
        
        [Test]
        public void GetIsChanged_NameChangeReverted_ReturnsFalse()
        {
            var observable = new ObservableGenericModel(_model);
            
            var changed = observable.GetIsChanged(m => m.Name);
            Assert.False(changed);
            Assert.False(observable.IsChanged);

            observable.Name = "New Name";
            changed = observable.GetIsChanged(m => m.Name);
            
            Assert.True(changed);
            Assert.True(observable.IsChanged);
            
            observable.Name = "Generic";
            changed = observable.GetIsChanged(m => m.Name);
            
            Assert.False(changed);
            Assert.False(observable.IsChanged);
        }

        [Test]
        public void AcceptChanges_WhenCalled_ShouldUpdateOriginalValue()
        {
            var observable = new ObservableGenericModel(_model);
            observable.Description = "New Description";
            Assert.AreEqual("New Description", observable.Description);
            Assert.AreEqual("Test observable model", observable.GetOriginalValue(x => x.Description));
            Assert.True(observable.IsChanged);
            
            observable.AcceptChanges();
            
            Assert.AreEqual("New Description", observable.Description);
            Assert.AreEqual("New Description", observable.GetOriginalValue(x => x.Description));
            Assert.False(observable.IsChanged);
        }
        
        [Test]
        public void RejectChanges_WhenCalled_ShouldUpdateOriginalValue()
        {
            var observable = new ObservableGenericModel(_model);
            observable.Description = "New Description";
            Assert.AreEqual("New Description", observable.Description);
            Assert.AreEqual("Test observable model", observable.GetOriginalValue(x => x.Description));
            Assert.True(observable.IsChanged);
            
            observable.RejectChanges();
            
            Assert.AreEqual("Test observable model", observable.Description);
            Assert.AreEqual("Test observable model", observable.GetOriginalValue(x => x.Description));
            Assert.False(observable.IsChanged);
        }
        
        [Test]
        public void AcceptChanges_ComplexProperty_ShouldUpdateOriginalValue()
        {
            var observable = new ObservableGenericModel(_model);
            observable.SubModel.Name = "New Name";
            Assert.AreEqual("New Name", observable.SubModel.Name);
            Assert.AreEqual("GenericSubModel", observable.SubModel.GetOriginalValue(x => x.Name));
            Assert.True(observable.IsChanged);
            
            observable.AcceptChanges();
            
            Assert.AreEqual("New Name", observable.SubModel.Name);
            Assert.AreEqual("New Name", observable.SubModel.GetOriginalValue(x => x.Name));
            Assert.False(observable.IsChanged);
        }
        
        [Test]
        public void RejectChanges_ComplexProperty_ShouldUpdateOriginalValue()
        {
            var observable = new ObservableGenericModel(_model);
            observable.SubModel.Name = "New Name";
            Assert.AreEqual("New Name", observable.SubModel.Name);
            Assert.AreEqual("GenericSubModel", observable.SubModel.GetOriginalValue(x => x.Name));
            Assert.True(observable.IsChanged);
            
            observable.RejectChanges();
            
            Assert.AreEqual("GenericSubModel", observable.SubModel.Name);
            Assert.AreEqual("GenericSubModel", observable.SubModel.GetOriginalValue(x => x.Name));
            Assert.False(observable.IsChanged);
        }

        [Test]
        public void IsChanged_ComplexPropertyChanged_ReturnsChanged()
        {
            var observable = new ObservableGenericModel(_model);
            observable.SubModel.Name = "New Name";
            Assert.True(observable.IsChanged);
            
            observable.SubModel.Name = "GenericSubModel";
            Assert.False(observable.IsChanged);
        }
        
        
    }
    
}