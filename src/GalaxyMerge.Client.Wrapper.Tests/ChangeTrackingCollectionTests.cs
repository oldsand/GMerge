using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GalaxyMerge.Client.Wrapper.Tests.Model;
using GalaxyMerge.Client.Wrappers.Base;
using NUnit.Framework;

namespace GalaxyMerge.Client.Wrapper.Tests
{
    public class ChangeTrackingCollectionTests
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
                    new TestItem {Id = 2, Item = "Item2", Value = 1.2},
                    new TestItem {Id = 3, Item = "Item3", Value = 1.3}
                }
            };
        }

        [Test]
        public void Constructor_ValidCollection_CollectionInitialized()
        {
            var wrapper = new TestModelWrapper(_model);

            Assert.NotNull(wrapper.TestItems);
            Assert.That(wrapper.TestItems, Has.Count.EqualTo(3));
        }

        [Test]
        public void Constructor_NullReference_ThrowsArgumentNullException()
        {
            _model.Items = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                var wrapper = new TestModelWrapper(_model);
            });
        }

        [Test]
        public void Add_ValidReference_AddsItemModelCollection()
        {
            var wrapper = new TestModelWrapper(_model);

            var item = new TestItemWrapper(new TestItem {Id = 4, Item = "Item4", Value = 1.4});
            wrapper.TestItems.Add(item);

            Assert.That(_model.Items, Contains.Item(item.Model));
        }
        
        [Test]
        public void Add_ValidReference_RaisesCollectionChangedEvent()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<NotifyCollectionChangedAction>();
            wrapper.TestItems.CollectionChanged += (_, e) => changed.Add(e.Action);
            
            var item = new TestItemWrapper(new TestItem {Id = 4, Item = "Item4", Value = 1.4});
            wrapper.TestItems.Add(item);

            Assert.That(changed, Contains.Item(NotifyCollectionChangedAction.Add));
        }
        
        [Test]
        public void Add_ValidReference_ItemContainedInAddedCollection()
        {
            var wrapper = new TestModelWrapper(_model);

            var item = new TestItemWrapper(new TestItem {Id = 4, Item = "Item4", Value = 1.4});
            wrapper.TestItems.Add(item);

            Assert.That(wrapper.TestItems.Added.Contains(item));
        }
        
        [Test]
        public void Add_ValidReference_IsChangedSetOnRoot()
        {
            var wrapper = new TestModelWrapper(_model);

            var item = new TestItemWrapper(new TestItem {Id = 4, Item = "Item4", Value = 1.4});
            wrapper.TestItems.Add(item);

            Assert.That(wrapper.IsChanged);
        }

        [Test]
        public void Remove_ValidReference_RemovesItemModelCollection()
        {
            var wrapper = new TestModelWrapper(_model);

            var item = wrapper.TestItems.FirstOrDefault();
            wrapper.TestItems.Remove(item);

            Assert.False(_model.Items.Contains(item?.Model));
        }
        
        [Test]
        public void Remove_ValidReference_RaisesCollectionChangedEvent()
        {
            var wrapper = new TestModelWrapper(_model);
            var changed = new List<NotifyCollectionChangedAction>();
            wrapper.TestItems.CollectionChanged += (_, e) => changed.Add(e.Action);
            
            var item = wrapper.TestItems.FirstOrDefault();
            wrapper.TestItems.Remove(item);

            Assert.That(changed, Contains.Item(NotifyCollectionChangedAction.Remove));
        }
        
        [Test]
        public void Remove_ValidReference_ItemContainedInRemovedCollection()
        {
            var wrapper = new TestModelWrapper(_model);

            var item = wrapper.TestItems.First();
            wrapper.TestItems.Remove(item);

            Assert.That(wrapper.TestItems.Removed.Contains(item));
        }

        [Test]
        public void Update_ValidReference_UpdatedModelItem()
        {
            var wrapper = new TestModelWrapper(_model);
            var item = wrapper.TestItems.First();

            item.Value = 1.5;
            
            Assert.AreEqual(wrapper.TestItems.First().Value, _model.Items.First().Value);
        }
        
        [Test]
        public void AcceptChanges_AfterAddingItem_UpdatedCollectionAsExpected()
        {
            var wrapper = new TestModelWrapper(_model);
            var item = new TestItemWrapper(new TestItem {Id = 4, Item = "Item4", Value = 1.4});
            wrapper.TestItems.Add(item);
            
            wrapper.AcceptChanges();

            Assert.True(wrapper.TestItems.Contains(item));
            Assert.True(wrapper.Model.Items.Contains(item.Model));
            Assert.IsEmpty(wrapper.TestItems.Added);
            Assert.IsEmpty(wrapper.TestItems.Modified);
            Assert.IsEmpty(wrapper.TestItems.Removed);
        }
        
        [Test]
        public void RejectChanged_AfterAddingItem_RemovesFromExpectedCollections()
        {
            var wrapper = new TestModelWrapper(_model);
            var collection = new ChangeTrackingCollection<TestItemWrapper>(_model.Items.Select(i => new TestItemWrapper(i)).ToList());
            var item = new TestItemWrapper(new TestItem {Id = 4, Item = "Item4", Value = 1.4});
            collection.Add(item);
            
            collection.RejectChanges();

            Assert.False(wrapper.TestItems.Contains(item));
            Assert.False(wrapper.Model.Items.Contains(item.Model));
            Assert.False(wrapper.TestItems.Added.Contains(item));
        }
        
        [Test]
        public void ShouldRejectChanges()
        {
            var items = new List<TestItemWrapper>
            {
                new TestItemWrapper( new TestItem() {Id = 1, Item = "Item1"}),
                new TestItemWrapper( new TestItem() {Id = 1, Item = "Item2"}),
                new TestItemWrapper( new TestItem() {Id = 1, Item = "Item3"})
            };
            
            var toModify = items.First();
            var toRemove = items.Skip(1).First();
            var toAdd = new TestItemWrapper(new TestItem() { Item = "ADDED" });

            var c = new ChangeTrackingCollection<TestItemWrapper>(items);

            c.Add(toAdd);
            c.Remove(toRemove);
            toModify.Item = "MODIFIED";
            Assert.AreEqual("MODIFIED", toModify.Item);

            Assert.AreEqual(3, c.Count);
            Assert.AreEqual(1, c.Added.Count);
            Assert.AreEqual(1, c.Modified.Count);
            Assert.AreEqual(1, c.Removed.Count);

            c.RejectChanges();

            Assert.AreEqual(3, c.Count);
            Assert.IsTrue(c.Contains(toModify));
            Assert.IsTrue(c.Contains(toRemove));

            Assert.AreEqual(0, c.Added.Count);
            Assert.AreEqual(0, c.Modified.Count);
            Assert.AreEqual(0, c.Removed.Count);

            Assert.IsFalse(toModify.IsChanged);
            Assert.AreEqual("Item1", toModify.Item);

            Assert.IsFalse(c.IsChanged);
        }
    }
}