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
        public void ModelProperty_SetToInstanceFromNull_ModelGetsUpdated()
        {
            var complexType = _model.ComplexType;
            _model.ComplexType = null;
            var wrapper = new TestModelWrapper(_model);
            Assert.Null(wrapper.ComplexType.Model);

            wrapper.ComplexType = new TestComplexTypeWrapper(complexType);
            Assert.NotNull(wrapper.ComplexType.Model);
            Assert.AreEqual(wrapper.ComplexType.Id, complexType.Id);
            Assert.AreEqual(wrapper.ComplexType.Name, complexType.Name);
        }

        [Test]
        public void PropertyChanged_SetComplexModel_ShouldRaiseEvent()
        {
            var complexType = _model.ComplexType;
            _model.ComplexType = null;
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.ComplexType.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(complexType);
            
            Assert.IsNotEmpty(changed);
        }

        [Test]
        public void PropertyChanged_SetComplexPropertyWithNewInstance_ShouldRaiseEvent()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<string>();
            wrapper.ComplexType.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ComplexType = new TestComplexTypeWrapper(new TestComplexType() { Id = 2, Name = "SomeName"});
            
            Assert.IsNotEmpty(changed);
        }
    }
}