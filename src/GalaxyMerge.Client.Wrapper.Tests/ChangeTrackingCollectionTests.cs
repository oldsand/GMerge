using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Client.Wrapper.Tests.Model;
using GalaxyMerge.Client.Wrappers.Base;
using NUnit.Framework;

namespace GalaxyMerge.Client.Wrapper.Tests
{
    public class ChangeTrackingCollectionTests
    {
        private List<SubModelWrapper> _subModels;

        [SetUp]
        public void Setup()
        {
            _subModels = new List<SubModelWrapper>
            {
                new SubModelWrapper(new SubModel {Id=1, Name = "Item1", Value = 1.1f}),
                new SubModelWrapper(new SubModel {Id=2, Name = "Item2", Value = 1.2f}),
                new SubModelWrapper(new SubModel {Id=2, Name = "Item3", Value = 1.3f})
            };
        }

        [Test]
        public void Add_WhenCalled_ReturnsItemToAddedCollection()
        {
            var collection = new ChangeTrackingCollection<SubModelWrapper>(_subModels);
            Assert.AreEqual(3, collection.Count);
            Assert.False(collection.IsChanged);

            var item = new SubModelWrapper(new SubModel() {Id = 4, Name = "Item4", Value = 1.4f});
            collection.Add(item);
            Assert.AreEqual(4, collection.Count);
            Assert.AreEqual(1, collection.Added.Count);
            Assert.AreEqual(0, collection.Removed.Count);
            Assert.AreEqual(0, collection.Modified.Count);
            Assert.AreEqual(item, collection.Added.First());
            Assert.True(collection.IsChanged);
        }
        
        [Test]
        public void Remove_WhenCalled_ReturnsItemToRemovedCollection()
        {
            var collection = new ChangeTrackingCollection<SubModelWrapper>(_subModels);
            Assert.AreEqual(3, collection.Count);
            Assert.False(collection.IsChanged);

            var item = collection.First();
            collection.Remove(item);
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(0, collection.Added.Count);
            Assert.AreEqual(1, collection.Removed.Count);
            Assert.AreEqual(0, collection.Modified.Count);
            Assert.AreEqual(item, collection.Removed.First());
            Assert.True(collection.IsChanged);
        }
    }
}