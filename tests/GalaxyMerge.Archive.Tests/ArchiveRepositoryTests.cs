using System.Xml.Linq;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Core.Extensions;
using NUnit.Framework;

namespace GalaxyMerge.Archive.Tests
{
    [TestFixture]
    public class ArchiveRepositoryTests
    {
        private const string GalaxyName = "Sandbox";
        
        [Test]
        public void Constructor_WhenCalled_ReturnsNotNull()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            Assert.NotNull(repo);
        }

        [Test]
        public void AddInfo_WhenCalled_AddsInfo()
        {
            using var repo = new ArchiveRepository(GalaxyName);
        }

        [Test]
        public void AddEntry_WhenCalled_AddsEntry()
        {
            using var repo = new ArchiveRepository(GalaxyName);

            var data = new XElement("SomeObject", "SomeData");
            var entry = new ArchiveEntry(2345, "SomeTag", 34, "$UserDefined", data.ToByteArray());
            
            repo.AddEntry(entry);
            repo.Save();

            var result = repo.GetEntry(entry.EntryId);
            Assert.NotNull(result);
            Assert.AreEqual(2345, result.ObjectId);
            Assert.AreEqual("SomeTag", result.TagName);
            Assert.AreEqual(34, result.Version);
            Assert.AreEqual("$UserDefined", result.BaseType);
        }

        [Test]
        public void GetLatest_WhenCalled_ReturnsSingleEntry()
        {
            using var repo = new ArchiveRepository(GalaxyName);

            var result = repo.GetLatest("SomeTag");
            
            Assert.NotNull(result);
        }
    }
}