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
        public void Constructor_ValidObject_ReturnsInstance()
        {
            var wrapper = new TestModelWrapper(_model);

            Assert.NotNull(wrapper);
            Assert.NotNull(wrapper.Model);
            Assert.NotNull(wrapper.ComplexType);
            //Assert.NotNull(wrapper.TestItems);
        }

        [Test]
        public void Constructor_ValidObject_ModeMatchesWrapper()
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
            //Assert.Null(wrapper.ComplexType.Model);
        }

        [Test]
        public void SetValue_NameProperty_WrapperMatchedModel()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.Name = "New Name";
            
            Assert.AreEqual(wrapper.Name, _model.Name);
        }
        
        [Test]
        public void SetValue_NameProperty_IsChangedReturnsTrue()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.Name = "New Name";
            
            Assert.True(wrapper.IsChanged);
        }
        
        [Test]
        public void SetValue_NameProperty_RaisesPropertyChanged()
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
        public void SetValue_SetToInstanceFromNull_WrapperMatchesModel()
        {
            var complexType = _model.ComplexType;
            _model.ComplexType = null;
            var wrapper = new TestModelWrapper(_model);
            Assert.Null(wrapper.ComplexType.Model);

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
        public void SetValue_ComplexFromNullToInstance_ShouldRaiseEvent()
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
        public void SetValue_ComplexNewInstanceAndProperties_WrapperMatchesModel()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 2, Name = "SomeName"});

            Assert.AreSame(wrapper.ComplexType.Model, _model.ComplexType);
            Assert.AreEqual(wrapper.ComplexType.Id, _model.ComplexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, _model.ComplexType.Name);
        }

        [Test]
        public void SetValue_ComplexNewInstanceNewValues_DoesNotRaiseEventsOrIsChangedForThatType()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.ComplexType.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 2, Name = "SomeName"});

            Assert.False(wrapper.ComplexType.IsChanged);
            Assert.IsEmpty(changed);
        }

        [Test]
        public void SetValue_ComplexNewInstanceSameProperties_WrapperMatchesModel()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 1, Name = "Complex"});

            Assert.AreSame(wrapper.ComplexType.Model, _model.ComplexType);
            Assert.AreEqual(wrapper.ComplexType.Id, _model.ComplexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, _model.ComplexType.Name);
        }

        [Test]
        public void SetValue_ComplexNewInstanceSameProperties_IsChangedReturnsFalse()
        {
            var wrapper = new TestModelWrapper(_model);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 1, Name = "Complex"});

            Assert.False(wrapper.IsChanged);
        }

        [Test]
        public void SetValue_ComplexNewInstanceSameProperties_DoesNotRaisePropertyChanged()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 1, Name = "Complex"});

            Assert.IsEmpty(changed);
        }

        [Test]
        public void SetValue_ComplexDifferentThenBack_IsChangedBecomesFalse()
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
        public void PropertyChanged_SetComplexPropertyWithNewInstance_ShouldRaiseEventOnRoot()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType {Id = 2, Name = "SomeName"});

            Assert.IsNotEmpty(changed);
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
        public void RequiredTest_TrackingObjectProperty_PerformsAsExpected()
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
    }
}