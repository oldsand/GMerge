using System.Collections.Generic;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Client.Wrappers;
using NUnit.Framework;

namespace GalaxyMerge.Client.Wrapper.Tests
{
    public class ResourceEntryWrapperTests
    {
        private ResourceEntry _model;

        [SetUp]
        public void Setup()
        {
            _model = new ResourceEntry("Some Name", ResourceType.Connection);
        }

        [Test]
        public void Constructor_ValidObject_ReturnsExpectedInstance()
        {
            var wrapper = new ResourceEntryWrapper(_model);

            Assert.NotNull(wrapper);
            Assert.NotNull(wrapper.Model);
            Assert.NotNull(wrapper.Connection);
            Assert.Null(wrapper.Archive);
            Assert.Null(wrapper.Directory);
            Assert.AreEqual(wrapper.ResourceName, _model.ResourceName);
            Assert.AreEqual(wrapper.ResourceDescription, _model.ResourceDescription);
            Assert.AreEqual(wrapper.ResourceType, _model.ResourceType);
            Assert.AreEqual(wrapper.AddedOn, _model.AddedOn);
            Assert.AreEqual(wrapper.AddedBy, _model.AddedBy);
            Assert.AreSame(wrapper.Model, _model);
        }

        [Test]
        public void SetValue_ResourceName_SetsThePropertyAndRaisesEvents()
        {
            var wrapper = new ResourceEntryWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ResourceName = "New Resource Name";

            Assert.AreEqual("New Resource Name", wrapper.ResourceName);
            Assert.AreEqual(wrapper.ResourceName, _model.ResourceName);
            Assert.That(changed, Contains.Item(nameof(wrapper.ResourceName)));
            Assert.That(changed, Contains.Item("ResourceNameIsChanged"));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsChanged)));
        }

        [Test]
        public void SetValue_ResourceDescription_SetsThePropertyAndRaisesEvents()
        {
            var wrapper = new ResourceEntryWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.ResourceDescription = "This is the description test";

            Assert.AreEqual("This is the description test", wrapper.ResourceDescription);
            Assert.AreEqual(wrapper.ResourceDescription, _model.ResourceDescription);
            Assert.That(changed, Contains.Item(nameof(wrapper.ResourceDescription)));
            Assert.That(changed, Contains.Item("ResourceDescriptionIsChanged"));
            Assert.That(changed, Contains.Item(nameof(wrapper.IsChanged)));
        }

        [Test]
        public void SetValue_ConnectionProperties_SetsThePropertyAndRaisesEvents()
        {
            var wrapper = new ResourceEntryWrapper(_model);
            var changed = new List<string>();
            wrapper.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            wrapper.Connection.NodeName = "Node";
            wrapper.Connection.GalaxyName = "Galaxy";

            Assert.AreEqual(wrapper.Connection.NodeName, "Node");
            Assert.AreEqual(wrapper.Connection.GalaxyName, "Galaxy");
            Assert.That(changed, Contains.Item(nameof(wrapper.IsChanged)));
        }

        [Test]
        public void ResourcePath_WhenCalled_GetsExpectedResourcePath()
        {
            var wrapper = new ResourceEntryWrapper(_model);
            wrapper.Connection.NodeName = "NodeName";
            wrapper.Connection.GalaxyName = "GalaxyName";

            Assert.AreEqual(@"NodeName\GalaxyName", wrapper.ResourcePath);
        }
    }
}