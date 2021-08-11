using System;
using System.IO;
using System.Linq;
using GCommon.Archiving.Repositories;
using GCommon.Core.Utilities;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Archiving;
using GCommon.Primitives.Enumerations;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace GCommon.Archiving.IntegrationTests
{
    [TestFixture]
    public class InclusionSettingRepositoryTests
    {
        private SqliteConnectionStringBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new SqliteConnectionStringBuilder {DataSource = @"\TestArchive.db"};

            var archive = new Archive("TestArchive");
            
            var archiveBuilder = new ArchiveBuilder();
            archiveBuilder.Build(archive, _builder.ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            var fileName = _builder.DataSource;
            File.Delete(fileName);
        }
        
        [Test]
        public void IsIncluded_TemplateWithOptionAll_ReturnsTrue()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(1, "$SomeTag", 1, Template.UserDefined);
            
            var result = repo.Inclusions.IsIncluded(archiveObject);

            Assert.True(result);
        }
        
        [Test]
        public void IsIncluded_TemplateWithOptionSelect_ReturnsFalse()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var archiveObject = new ArchiveObject(1, "SomeTag", 1, Template.Area);
            
            var result = repo.Inclusions.IsIncluded(archiveObject);

            Assert.False(result);
        }
        
        [Test]
        public void IsIncluded_TemplateWithOptionNone_ReturnsFalse()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            repo.Inclusions.Configure(Template.InTouchViewApp, InclusionOption.None, false);
            var archiveObject = new ArchiveObject(1, "SomeTag", 31, Template.InTouchViewApp);
            
            var result = repo.Inclusions.IsIncluded(archiveObject);

            Assert.False(result);
        }
        
        [Test]
        public void IsIncluded_Null_ThrowsArgumentNullException()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = repo.Inclusions.IsIncluded(null);
            }, "archiveObject can not be null");
        }
        
        [Test]
        public void Get_ValidTemplate_ReturnsExpectSetting()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            var result = repo.Inclusions.Get(Template.Galaxy);

            Assert.NotNull(result);
            Assert.AreEqual(Template.Galaxy, result.Template);
        }
        
        [Test]
        public void Get_Null_ThrowArgumentNullException()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);

            Assert.Throws<ArgumentNullException>(() =>
            {
                var result = repo.Inclusions.Get(null);
            }, "archiveObject can not be null");
        }
        
        [Test]
        public void GetAll_WhenCalled_ReturnsNumbersOfExpectedTemplates()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            var count = Template.List.Count;

            var templates = repo.Inclusions.GetAll().ToList();
            
            Assert.That(templates, Has.Count.EqualTo(count));
        }

        [Test]
        public void FindByOption_OptionsAll_ReturnsAllInclusionsWithAll()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            
            var settings = repo.Inclusions.FindByOption(InclusionOption.All).ToList();
            
            Assert.That(settings, Has.Count.EqualTo(2));
        }

        [Test]
        public void Configure_SetOptionNoneIncludeFalse_ReturnsExpectedValues()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            
            repo.Inclusions.Configure(Template.ClientControl, InclusionOption.None, false);
            repo.Save();

            var result = repo.Inclusions.Get(Template.ClientControl);
            
            Assert.AreEqual(InclusionOption.None, result.InclusionOption);
            Assert.False(result.IncludeInstances);
        }
        
        [Test]
        public void Configure_SetOptionAllIncludeTrue_ReturnsExpectedValues()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            
            repo.Inclusions.Configure(Template.OpcClient, InclusionOption.All, true);
            repo.Save();

            var result = repo.Inclusions.Get(Template.OpcClient);
            
            Assert.AreEqual(InclusionOption.All, result.InclusionOption);
            Assert.True(result.IncludeInstances);
        }
        
        [Test]
        public void Configure_NullOperation_ReturnsFalse()
        {
            using var repo = new ArchiveRepository(_builder.ConnectionString);
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                repo.Events.Configure(null, false);
            }, "operation can not be null");
        }
    }
}