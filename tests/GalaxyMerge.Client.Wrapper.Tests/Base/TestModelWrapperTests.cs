using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GalaxyMerge.Client.Wrapper.Tests.Base
{
    [TestFixture]
    public class TestModelWrapperTests
    {
        private TestModel _model;

        [SetUp]
        public void Setup()
        {
            _model = new TestModel
            {
                Id = 2,
                Name = "Generic",
                Description = "This is a test",
                ComplexType = new TestComplexType
                {
                    Id = 1,
                    Name = "Complex",
                },
                Items = new List<TestItem>
                {
                    new TestItem {Id = 1, Item = "Item1", Value = 1.1},
                    new TestItem {Id = 2, Item = "Item2", Value = 1.3},
                    new TestItem {Id = 3, Item = "Item3", Value = 1.3}
                }
            };
        }

        [Test]
        public void Constructor_ValidReference_ReturnsInstanceWithInitializedMembers()
        {
            var wrapper = new TestModelWrapper(_model);

            Assert.NotNull(wrapper);
            Assert.NotNull(wrapper.Model);
            Assert.AreEqual(_model, wrapper.Model);
            Assert.NotNull(wrapper.ComplexType);
            Assert.NotNull(wrapper.ComplexType.Model);
            Assert.AreEqual(_model.ComplexType, wrapper.ComplexType.Model);
            Assert.NotNull(wrapper.TestItems);
            Assert.AreEqual(_model.Items.Count, wrapper.TestItems.Count);
            Assert.IsTrue(_model.Items.All(item =>
                wrapper.TestItems.Any(itemWrapper => itemWrapper.Model == item)));
        }
        
        [Test]
        public void Constructor_NullReference_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var wrapper = new TestModelWrapper(null);
            });
        }

        [Test]
        public void Constructor_ValidReference_ModeMatchesWrapper()
        {
            var wrapper = new TestModelWrapper(_model);
            
            Assert.AreSame(wrapper.Model, _model);
            Assert.AreEqual(wrapper.Id, _model.Id);
            Assert.AreEqual(wrapper.Name, _model.Name);
            Assert.AreEqual(wrapper.Description, _model.Description);
            Assert.AreSame(wrapper.ComplexType.Model, _model.ComplexType);
            Assert.AreEqual(wrapper.ComplexType.Id, _model.ComplexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, _model.ComplexType.Name);

            foreach (var testItem in wrapper.TestItems)
            {
                var item = _model.Items.Single(x => x.Id == testItem.Id);
                Assert.AreSame(testItem.Model, item);
                Assert.AreEqual(testItem.Id, item.Id);
                Assert.AreEqual(testItem.Item, item.Item);
                Assert.AreEqual(testItem.Value, item.Value);
            }
        }

        [Test]
        public void Constructor_NullComplexType_ReturnsInstanceWithNullWrapper()
        {
            _model.ComplexType = null;
            var wrapper = new TestModelWrapper(_model);
            Assert.NotNull(wrapper);
            Assert.NotNull(wrapper.Model);
            Assert.Null(wrapper.ComplexType);
        }

        [Test]
        public void SetValue_NamePropertyNewValue_WrapperMatchesModel()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.Name = "New Name";
            
            Assert.AreEqual("New Name", _model.Name);
            Assert.AreEqual(wrapper.Name, _model.Name);
        }
        
        [Test]
        public void SetValue_NamePropertyNewValue_IsChangedReturnsTrue()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.Name = "New Name";
            
            Assert.True(wrapper.IsChanged);
        }
        
        [Test]
        public void SetValue_NamePropertyNewValue_RaisesPropertyChanged()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
            
            wrapper.Name = "New Name";
            
            Assert.IsNotEmpty(changed);
            Assert.That(changed, Contains.Item(nameof(wrapper.Name)));
            Assert.That(changed, Contains.Item("NameIsChanged"));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsChanged)));
        }
        
        [Test]
        public void SetValue_NamePropertyNewValueThenBack_IsChangedReturnsFalse()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.Name = "New Name";
            wrapper.Name = "Generic";
            
            Assert.False(wrapper.IsChanged);
        }
        
        [Test]
        public void SetValue_NamePropertyNewValueThenBack_RaisesPropertyChanged()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
            
            wrapper.Name = "New Name";
            changed.Clear();
            wrapper.Name = "Generic";
            
            Assert.IsNotEmpty(changed);
            Assert.That(changed, Contains.Item(nameof(wrapper.Name)));
            Assert.That(changed, Contains.Item("NameIsChanged"));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsChanged)));
        }
        
        [Test]
        public void SetValue_NamePropertySameValue_IsChangedReturnsFalse()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.Name = "Generic";
            
            Assert.False(wrapper.IsChanged);
        }
        
        [Test]
        public void SetValue_NamePropertySameValue_DoesNotRaisePropertyChanged()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
            
            wrapper.Name = "Generic";
            
            Assert.IsEmpty(changed);
        }

        [Test]
        public void SetValue_SimpleNonModelProperty_ReturnsSetValue()
        {
            var wrapper = new TestModelWrapper(_model);
            
            wrapper.CreatedOn = DateTime.Today;
            
            Assert.AreEqual(DateTime.Today, wrapper.CreatedOn);
        }
        
        [Test]
        public void SetValue_CreatedOn_DoesNotRegisterChange()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
            
            wrapper.CreatedOn = DateTime.Now;
            
            Assert.False(wrapper.IsChanged);
            Assert.IsEmpty(changed);
        }

        [Test]
        public void SetValue_ComplexProperty_ModelMatchesWrapper()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.ComplexType.Name = "Test Name";
            
            Assert.AreEqual("Test Name", _model.ComplexType.Name);
            Assert.AreEqual(_model.ComplexType.Name, wrapper.ComplexType.Name);
        }
        
        [Test]
        public void SetValue_ComplexProperty_RaisesPropertyChangedOnRootAndChild()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
            wrapper.ComplexType.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
            
            wrapper.ComplexType.Name = "Test Name";
            
            Assert.IsNotEmpty(changed);
            Assert.True(changed.Contains(nameof(wrapper.ComplexType.Name)));
            Assert.True(changed.Contains(nameof(wrapper.ComplexType.IsChanged)));
            Assert.True(changed.Contains(nameof(wrapper.IsChanged)));
        }
        
        [Test]
        public void SetValue_ComplexPropertyNullToInstance_WrapperMatchesModel()
        {
            var complexType = _model.ComplexType;
            _model.ComplexType = null;
            var wrapper = new TestModelWrapper(_model);
            Assert.Null(wrapper.ComplexType);

            wrapper.ComplexType = new TestComplexTypeWrapper(complexType);

            Assert.NotNull(wrapper.ComplexType);
            Assert.NotNull(wrapper.ComplexType.Model);
            Assert.NotNull(_model.ComplexType);
            Assert.AreEqual(wrapper.ComplexType.Id, complexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, complexType.Name);
            Assert.AreSame(wrapper.ComplexType.Model, _model.ComplexType);
            Assert.AreEqual(wrapper.ComplexType.Id, _model.ComplexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, _model.ComplexType.Name);
        }
        
        [Test]
        public void SetValue_ComplexPropertyNullToInstance_ShouldRaiseEvent()
        {
            var complexType = _model.ComplexType;
            _model.ComplexType = null;
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(complexType);

            Assert.IsNotEmpty(changed);
            Assert.That(changed, Contains.Item(nameof(wrapper.ComplexType)));
            Assert.That(changed, Contains.Item("ComplexTypeIsChanged"));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsChanged)));
        }
        
        [Test]
        public void SetValue_ComplexPropertyInstanceToNull_WrapperMatchesModel()
        {
            var wrapper = new TestModelWrapper(_model);
            Assert.NotNull(wrapper.ComplexType);
            Assert.NotNull(wrapper.ComplexType.Model);

            wrapper.ComplexType = null;

            Assert.Null(wrapper.ComplexType);
            Assert.Null(_model.ComplexType);
        }
        
        [Test]
        public void SetValue_ComplexPropertyNewInstanceAndProperties_WrapperMatchesModel()
        {
            var wrapper = new TestModelWrapper(_model);
            
            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 2, Name = "SomeName"});

            Assert.AreSame(wrapper.ComplexType.Model, _model.ComplexType);
            Assert.AreEqual(wrapper.ComplexType.Id, _model.ComplexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, _model.ComplexType.Name);
        }

        [Test]
        public void SetValue_ComplexPropertyNewInstanceAndProperties_ShouldRaiseEventOnRoot()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 2, Name = "SomeName"});

            Assert.IsNotEmpty(changed);
        }

        [Test]
        public void SetValue_ComplexPropertyNewInstanceAndProperties_DoesNotRaiseEventsOrIsChangedForThatType()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.ComplexType.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 2, Name = "SomeName"});

            Assert.False(wrapper.ComplexType.IsChanged);
            Assert.IsEmpty(changed);
        }

        [Test]
        public void SetValue_ComplexPropertyNewInstanceSameProperties_WrapperMatchesModel()
        {
            var wrapper = new TestModelWrapper(_model);
            
            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 1, Name = "Complex"});

            Assert.AreSame(wrapper.ComplexType.Model, _model.ComplexType);
            Assert.AreEqual(wrapper.ComplexType.Id, _model.ComplexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, _model.ComplexType.Name);
        }

        [Test]
        public void SetValue_ComplexPropertyNewInstanceSamePropertiesThenUpdate_WrapperMatchesModel()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 1, Name = "Complex"});
            Assert.AreSame(wrapper.ComplexType.Model, _model.ComplexType);
            Assert.AreEqual(wrapper.ComplexType.Id, _model.ComplexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, _model.ComplexType.Name);
            
            wrapper.ComplexType.Name = "new Name";
            Assert.AreEqual(wrapper.ComplexType.Id, _model.ComplexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, _model.ComplexType.Name);
        }

        [Test]
        public void SetValue_ComplexPropertyNewInstanceSameProperties_IsChangedReturnsFalse()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 1, Name = "Complex"});

            Assert.False(wrapper.IsChanged);
        }

        [Test]
        public void SetValue_ComplexPropertyNewInstanceSameProperties_DoesNotRaisePropertyChanged()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 1, Name = "Complex"});

            Assert.IsEmpty(changed);
        }

        [Test]
        public void SetValue_ComplexPropertyDifferentThenBack_ReturnsExpectedValues()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 2, Name = "SomeName"});
            Assert.True(wrapper.IsChanged);
            Assert.That(changed, Contains.Item(nameof(wrapper.ComplexType)));
            Assert.That(changed, Contains.Item("ComplexTypeIsChanged"));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsChanged)));

            changed.Clear();
            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 1, Name = "Complex"});

            Assert.False(wrapper.IsChanged);
            Assert.That(changed, Contains.Item(nameof(wrapper.ComplexType)));
            Assert.That(changed, Contains.Item("ComplexTypeIsChanged"));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsChanged)));
        }

        [Test]
        public void CollectionPropertyAdd_ValidReference_WrapperMatchedModel()
        {
            var wrapper = new TestModelWrapper(_model);
            var item = new TestItemWrapper(new TestItem() {Id = 123, Item = "TestAdd", Value = 1.234});
            
            wrapper.TestItems.Add(item);
            
            Assert.AreEqual(_model.Items.Count, wrapper.TestItems.Count);
            Assert.True(_model.Items.All(i => wrapper.TestItems.Any(w => w.Model == i)));
        }
        
        [Test]
        public void CollectionPropertyRemove_ValidReference_WrapperMatchedModel()
        {
            var wrapper = new TestModelWrapper(_model);
            var item = wrapper.TestItems.First();
            
            wrapper.TestItems.Remove(item);
            
            Assert.AreEqual(_model.Items.Count, wrapper.TestItems.Count);
            Assert.True(_model.Items.All(i => wrapper.TestItems.Any(w => w.Model == i)));
        }
        
        [Test]
        public void CollectionPropertyClear_WhenCalled_WrapperMatchedModel()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.TestItems.Clear();
            
            Assert.AreEqual(_model.Items.Count, wrapper.TestItems.Count);
            Assert.True(_model.Items.All(i => wrapper.TestItems.Any(w => w.Model == i)));
        }

        [Test]
        public void GetOriginalValue_UnChangedProperty_ReturnsCurrentValue()
        {
            var wrapper = new TestModelWrapper(_model);

            var originalValue = wrapper.GetOriginalValue(m => m.Name);
            
            Assert.AreEqual(wrapper.Name, originalValue);
        }
        
        [Test]
        public void GetOriginalValue_ChangedProperty_ReturnsOriginalValue()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.Name = "New Value";
            
            var originalValue = wrapper.GetOriginalValue(m => m.Name);
            
            Assert.AreNotEqual(wrapper.Name, originalValue);
            Assert.AreEqual("Generic", originalValue);
        }
        
        [Test]
        public void GetOriginalValue_ChangedPropertyMultipleTimes_ReturnsOriginalValue()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.Name = "New Value";
            
            var originalValue = wrapper.GetOriginalValue(w => w.Name);
            
            Assert.AreNotEqual(wrapper.Name, originalValue);
            Assert.AreEqual("Generic", originalValue);

            wrapper.Name = "Some Other Value";

            originalValue = wrapper.GetOriginalValue(w => w.Name);
            
            Assert.AreNotEqual(wrapper.Name, originalValue);
            Assert.AreEqual("Generic", originalValue);
        }
        
        [Test]
        public void GetOriginalValue_ChangedPropertyChangeBack_ReturnsOriginalValue()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.Name = "New Value";
            
            var originalValue = wrapper.GetOriginalValue(w => w.Name);
            
            Assert.AreNotEqual(wrapper.Name, originalValue);
            Assert.AreEqual("Generic", originalValue);

            wrapper.Name = "Generic";

            Assert.AreEqual(wrapper.Name, originalValue);
            Assert.AreEqual("Generic", originalValue);
        }

        [Test]
        public void GetIsRequired_EmptyRequiredProperty_ReturnsTrue()
        {
            _model.Name = "";
            var wrapper = new TestModelWrapper(_model);
            
            Assert.True(wrapper.GetIsRequired(m => m.Name));
        }
        
        [Test]
        public void GetIsRequired_NonEmptyRequiredProperty_ReturnsFalse()
        {
            var wrapper = new TestModelWrapper(_model);
            
            Assert.False(wrapper.GetIsRequired(m => m.Name));
        }
        
        [Test]
        public void GetIsRequired_NonRequiredProperty_ReturnsFalse()
        {
            var wrapper = new TestModelWrapper(_model);
            
            Assert.False(wrapper.GetIsRequired(m => m.Description));
        }
        
        [Test]
        public void GetIsRequired_RequiredPropertySetEmptyAndBack_ReturnsExpectedValue()
        {
            var wrapper = new TestModelWrapper(_model);
            Assert.False(wrapper.GetIsRequired(m => m.Name));

            wrapper.Name = "";
            Assert.True(wrapper.GetIsRequired(m => m.Name));
            
            wrapper.Name = "NonEmpty";
            Assert.False(wrapper.GetIsRequired(m => m.Name));
        }

        [Test]
        public void IsChanged_AfterInitialization_ReturnsFalse()
        {
            var wrapper = new TestModelWrapper(_model);
            Assert.False(wrapper.IsChanged);
        }
        
        [Test]
        public void IsChanged_ChangedSimpleProperty_ReturnsTrue()
        {
            var wrapper = new TestModelWrapper(_model);
            wrapper.Description = "New Value";
            Assert.True(wrapper.IsChanged);
        }
        
        [Test]
        public void IsChanged_ChangedSimplePropertyChangeBack_ReturnsExpected()
        {
            var wrapper = new TestModelWrapper(_model);
            
            wrapper.Description = "New Value";
            Assert.True(wrapper.IsChanged);
            
            wrapper.Description = "This is a test";
            Assert.False(wrapper.IsChanged);
        }
        
        [Test]
        public void IsChanged_ChangedComplexProperty_ReturnsTrueForRootAndChild()
        {
            var wrapper = new TestModelWrapper(_model);
            wrapper.ComplexType.Name = "Changed Name";
            Assert.True(wrapper.IsChanged);
            Assert.True(wrapper.ComplexType.IsChanged);
        }
        
        [Test]
        public void IsChanged_ChangedCollectionProperty_ReturnsTrueForRootAndChild()
        {
            var wrapper = new TestModelWrapper(_model);
            wrapper.TestItems.First().Item = "Changed Item";
            Assert.True(wrapper.IsChanged);
            Assert.True(wrapper.TestItems.First().IsChanged);
        }
        
        [Test]
        public void HasRequired_InitialBlankProperty_ReturnsTrue()
        {
            _model.Name = string.Empty;
            var wrapper = new TestModelWrapper(_model);
            Assert.True(wrapper.HasRequired);
        }
        
        [Test]
        public void HasRequired_InitialBlankComplexProperty_ReturnsTrue()
        {
            _model.ComplexType.Name = string.Empty;
            var wrapper = new TestModelWrapper(_model);
            Assert.True(wrapper.HasRequired);
        }
        
        [Test]
        public void HasRequired_ChangingNameProperty_ReturnsExpected()
        {
            _model.Name = "";
            var wrapper = new TestModelWrapper(_model);
            Assert.True(wrapper.HasRequired);

            wrapper.Name = "NotEmpty";
            Assert.False(wrapper.HasRequired);
            
            wrapper.Name = null;
            Assert.True(wrapper.HasRequired);
        }

        [Test]
        public void HasRequired_UpdatingNameProperty_RaisesHasRequiredPropertyChanged()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.Name = "";
            Assert.True(changed.Contains(nameof(wrapper.HasRequired)));
            
            changed.Clear();
            wrapper.Name = null;
            Assert.False(changed.Contains(nameof(wrapper.HasRequired)));
            
            changed.Clear();
            wrapper.Name = "NotEmpty";
            Assert.True(changed.Contains(nameof(wrapper.HasRequired)));
        }

        [Test]
        public void HasRequired_TrackingObjectProperty_PerformsAsExpected()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
            
            Assert.False(wrapper.HasRequired);
            Assert.False(wrapper.ComplexType.GetIsRequired(m => m.Name));

            wrapper.ComplexType.Name = string.Empty;
            Assert.True(wrapper.HasRequired);
            Assert.True(wrapper.ComplexType.HasRequired);
            Assert.True(wrapper.ComplexType.GetIsRequired(m => m.Name));
            Assert.True(changed.Contains(nameof(wrapper.HasRequired)));
            
            changed.Clear();
            wrapper.ComplexType.Name = null;
            Assert.True(wrapper.HasRequired);
            Assert.True(wrapper.ComplexType.HasRequired);
            Assert.True(wrapper.ComplexType.GetIsRequired(m => m.Name));
            Assert.False(changed.Contains(nameof(wrapper.HasRequired)));
            
            changed.Clear();
            wrapper.ComplexType.Name = "NotEmpty";
            Assert.False(wrapper.HasRequired);
            Assert.False(wrapper.ComplexType.HasRequired);
            Assert.False(wrapper.ComplexType.GetIsRequired(m => m.Name));
            Assert.True(changed.Contains(nameof(wrapper.HasRequired)));
        }
        
        [Test]
        public void AcceptChanges_SimpleProperty_OriginalReturnsUpdatedAndIsChangedFalse()
        {
            var wrapper = new TestModelWrapper(_model);
            wrapper.Name = "New Name";
            Assert.AreEqual("New Name", wrapper.Name);
            Assert.AreEqual("Generic", wrapper.GetOriginalValue(w => w.Name));
            Assert.IsTrue(wrapper.IsChanged);

            wrapper.AcceptChanges();

            Assert.AreEqual("New Name", wrapper.Name);
            Assert.AreEqual("New Name", wrapper.GetOriginalValue(w => w.Name));
            Assert.IsFalse(wrapper.IsChanged);
        }

        [Test]
        public void RejectChanges_SimpleProperty_PropertyRevertedIsChangedFalse()
        {
            var wrapper = new TestModelWrapper(_model);
            wrapper.Name = "New Name";
            Assert.AreEqual("New Name", wrapper.Name);
            Assert.AreEqual("Generic", wrapper.GetOriginalValue(w => w.Name));
            Assert.IsTrue(wrapper.IsChanged);

            wrapper.RejectChanges();

            Assert.AreEqual("Generic", wrapper.Name);
            Assert.AreEqual("Generic", wrapper.GetOriginalValue(w => w.Name));
            Assert.IsFalse(wrapper.IsChanged);
        }
        
        [Test]
        public void AcceptChanges_ComplexProperty_OriginalReturnsUpdatedAndIsChangedFalse()
        {
            var wrapper = new TestModelWrapper(_model);
            
            wrapper.ComplexType.Name = "New Name";
            
            Assert.AreEqual("New Name", wrapper.ComplexType.Name);
            Assert.AreEqual("Complex", wrapper.ComplexType.GetOriginalValue(w => w.Name));
            Assert.True(wrapper.ComplexType.IsChanged);
            Assert.True(wrapper.IsChanged);

            wrapper.AcceptChanges();

            Assert.AreEqual("New Name", wrapper.ComplexType.Name);
            Assert.AreEqual("New Name", wrapper.ComplexType.GetOriginalValue(w => w.Name));
            Assert.False(wrapper.ComplexType.IsChanged);
            Assert.False(wrapper.IsChanged);
        }

        [Test]
        public void RejectChanges_ComplexProperty_PropertyRevertedIsChangedFalse()
        {
            var wrapper = new TestModelWrapper(_model);
            
            wrapper.ComplexType.Name = "New Name";
            
            Assert.AreEqual("New Name", wrapper.ComplexType.Name);
            Assert.AreEqual("Complex", wrapper.ComplexType.GetOriginalValue(w => w.Name));
            Assert.True(wrapper.ComplexType.IsChanged);
            Assert.True(wrapper.IsChanged);

            wrapper.RejectChanges();

            Assert.AreEqual("Complex", wrapper.ComplexType.Name);
            Assert.AreEqual("Complex", wrapper.ComplexType.GetOriginalValue(w => w.Name));
            Assert.False(wrapper.ComplexType.IsChanged);
            Assert.False(wrapper.IsChanged);
        }
    }
}