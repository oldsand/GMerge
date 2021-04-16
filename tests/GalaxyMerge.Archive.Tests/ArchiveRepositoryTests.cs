using System.Linq;
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
            var info = new ArchiveInfo(GalaxyName, 45, "Doesn't Matter", "1234.5678.9100");

            var current = repo.GetInfo();
            if (current != null)
            {
                repo.RemoveInfo(current);
                repo.Save();
            }

            repo.AddInfo(info);
            repo.Save();

            var result = repo.GetInfo();
            
            Assert.NotNull(result);
            Assert.AreEqual(result.GalaxyName, GalaxyName);
            Assert.AreEqual(result.VersionNumber, 45);
            Assert.AreEqual(result.CdiVersion, "Doesn't Matter");
            Assert.AreEqual(result.IsaVersion, "1234.5678.9100");
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

        [Test]
        public void FindByObjectId_WhenCalled_ReturnsEntryWithObjectId()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            
            var data = new XElement("SomeObject", "SomeData");
            var entry = new ArchiveEntry(53262, "TagName", 4, "$Area", data.ToByteArray());
            repo.AddEntry(entry);
            repo.Save();

            var results = repo.FindByObjectId(53262).ToList();

            Assert.IsNotEmpty(results);
            Assert.True(results.Any(x => x.ObjectId == 53262));
        }

        [Test]
        public void FindByTagName_WhenCalled_ReturnsObjectsWithTagName()
        {
            using var repo = new ArchiveRepository(GalaxyName);
            
            var data = new XElement("SomeObject", "SomeData");
            var entry = new ArchiveEntry(1234, "Single_Tag_Name", 4, "$Area", data.ToByteArray());
            repo.AddEntry(entry);
            repo.Save();

            var results = repo.FindByTagName("Single_Tag_Name").ToList();

            Assert.IsNotEmpty(results);
            Assert.True(results.Any(x => x.TagName == "Single_Tag_Name"));

        }
    }
}