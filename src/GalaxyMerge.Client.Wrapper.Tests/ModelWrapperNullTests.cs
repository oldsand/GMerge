using System.Collections.Generic;
using GalaxyMerge.Client.Wrapper.Tests.Model;
using NUnit.Framework;

namespace GalaxyMerge.Client.Wrapper.Tests
{
    [TestFixture]
    public class ModelWrapperNullTests
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
                }
            };
        }

        [Test]
        public void Constructor_ValidObject_ReturnsInstance()
        {
            var wrapper = new TestModelWrapper(_model);
            
            Assert.NotNull(wrapper);
            Assert.NotNull(wrapper.Model);
        }

        [Test]
        public void Constructor_NullComplexType_ReturnsInstanceWithNullWrapper()
        {
            _model.ComplexType = null;
            var wrapper = new TestModelWrapper(_model);
            Assert.NotNull(wrapper);
            Assert.NotNull(wrapper.Model);
            Assert.NotNull(wrapper.ComplexType);
            Assert.Null(wrapper.ComplexType.Model);
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
        public void SetValue_FromNullToInstance_ShouldRaiseEvent()
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
            Assert.That(changed, Contains.Item(nameof(wrapper.IsValid)));
        }
        
        [Test]
        public void SetValue_NewInstanceAndProperties_WrapperMatchesModel()
        {
            var wrapper = new TestModelWrapper(_model);
            
            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType { Id = 2, Name = "SomeName"});
            
            Assert.AreSame(wrapper.ComplexType.Model, _model.ComplexType);
            Assert.AreEqual(wrapper.ComplexType.Id, _model.ComplexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, _model.ComplexType.Name);
        }
        
                
        [Test]
        public void SetValue_NewInstanceNewValues_DoesNotRaiseEventsOrIsChangedForThatType()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.ComplexType.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType { Id = 2, Name = "SomeName"});
            
            Assert.False(wrapper.ComplexType.IsChanged);
            Assert.IsEmpty(changed);
        }
        
        [Test]
        public void SetValue_NewInstanceSameProperties_WrapperMatchesModel()
        {
            var wrapper = new TestModelWrapper(_model);
            
            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType { Id = 1, Name = "Complex"});
            
            Assert.AreSame(wrapper.ComplexType.Model, _model.ComplexType);
            Assert.AreEqual(wrapper.ComplexType.Id, _model.ComplexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, _model.ComplexType.Name);
            
        }
        
        [Test]
        public void SetValue_NewInstanceSameProperties_IsChangedReturnsFalse()
        {
            var wrapper = new TestModelWrapper(_model);
            
            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType { Id = 1, Name = "Complex"});
            
            Assert.False(wrapper.IsChanged);
        }
        
        [Test]
        public void SetValue_NewInstanceSameProperties_DoesNotRaisePropertyChanged()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);
            
            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType { Id = 1, Name = "Complex"});
            
            Assert.IsEmpty(changed);
        }
        
        [Test]
        public void SetValue_DifferentThenBack_IsChangedBecomesFalse()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType { Id = 2, Name = "SomeName"});
            Assert.True(wrapper.IsChanged);
            Assert.That(changed, Contains.Item(nameof(wrapper.ComplexType)));
            Assert.That(changed, Contains.Item("ComplexTypeIsChanged"));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsChanged)));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsValid)));
            
            changed.Clear();
            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType { Id = 1, Name = "Complex"});
            
            Assert.False(wrapper.IsChanged);
            Assert.That(changed, Contains.Item(nameof(wrapper.ComplexType)));
            Assert.That(changed, Contains.Item("ComplexTypeIsChanged"));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsChanged)));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsValid)));
        }

        [Test]
        public void PropertyChanged_SetComplexPropertyWithNewInstance_ShouldRaiseEvent()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType { Id = 2, Name = "SomeName"});
            
            Assert.IsNotEmpty(changed);
        }
    }
}