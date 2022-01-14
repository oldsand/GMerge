using System;
using System.Collections.Generic;
using System.IO;
using ArchestrA.GRAccess;
using AutoFixture;
using FluentAssertions;
using GCommon.Core.Enumerations;
using GCommon.Utilities;
using GServer.Archestra.Extensions;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests.ExtensionTests
{
    [TestFixture]
    public class GalaxyExtensionTests
    {
        private static IGalaxy _galaxy;
        private GRAccessAppClass _grAccess;

        public static IEnumerable<TestCaseData> TestCaseTemplateNames
        {
            get
            {
                yield return new TestCaseData(Template.Area.Name);
                yield return new TestCaseData(Template.UserDefined.Name);
                yield return new TestCaseData(Template.ViewEngine.Name);
                yield return new TestCaseData(Template.AppEngine.Name);
            }
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _grAccess = new GRAccessAppClass();
            _galaxy = _grAccess.QueryGalaxies()[TestConfig.GalaxyName];
            _galaxy.Login(string.Empty, string.Empty);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _galaxy.Logout();
            _galaxy = null;
        }

        [Test]
        public void DeepCheckIn_ValidTagNames_AllObjectsReturnCheckedOut()
        {
            var target = _galaxy.GetObjectByName(Known.Templates.ReactorSet.TagName);
            target.CheckOut();
            Assert.True(target.CheckoutStatus == ECheckoutStatus.checkedOutToMe);

            _galaxy.DeepCheckIn(Known.Templates.ReactorSet.TagName);

            Assert.True(target.CheckoutStatus == ECheckoutStatus.notCheckedOut);
        }

        [Test]
        public void GetObjectByName_KnownTemplateName_ReturnsExpectedInstance()
        {
            var tagName = Known.Templates.ReactorSet.TagName;

            var result = _galaxy.GetObjectByName(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.Tagname);
        }

        [Test]
        public void GetObjectByName_KnownInstanceName_ReturnsExpectedInstance()
        {
            const string tagName = Known.Instances.R31;

            var result = _galaxy.GetObjectByName(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.Tagname);
        }

        [Test]
        public void GetObjectByName_InvalidObject_ReturnsNull()
        {
            var fixture = new Fixture();

            var result = _galaxy.GetObjectByName(fixture.Create<string>());

            Assert.IsNull(result);
        }

        [Test]
        public void GetObjectByName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { _galaxy.GetObjectByName(null); });
        }

        [Test]
        [TestCaseSource(nameof(TestCaseTemplateNames))]
        public void GetTemplateByName_ValidTemplateName_ReturnsExpectedInstance(string tagName)
        {
            var result = _galaxy.GetTemplateByName(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.Tagname);
        }

        [Test]
        public void GetTemplateByName_InvalidTemplateName_ReturnsNull()
        {
            var fixture = new Fixture();

            var result = _galaxy.GetTemplateByName(fixture.Create<string>());

            Assert.IsNull(result);
        }

        [Test]
        public void GetTemplateByName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { _galaxy.GetTemplateByName(null); });
        }

        [Test]
        public void GetTemplatesByName_ValidTemplates_ReturnsExpectedCount()
        {
            var tagNames = new List<string>
            {
                Template.ViewEngine.Name,
                Template.AppEngine.Name,
                Template.WinPlatform.Name,
                Template.InTouchViewApp.Name
            };

            var results = _galaxy.GetTemplatesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsNotEmpty(actual);
            Assert.That(actual, Has.Count.EqualTo(4));
        }

        [Test]
        public void GetTemplatesByName_InvalidObject_ReturnsIsEmpty()
        {
            var fixture = new Fixture();
            var tagNames = fixture.CreateMany<string>(4);

            var results = _galaxy.GetTemplatesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetTemplatesByName_SomeRealSomeFake_ReturnsInstanceWithTagName()
        {
            var fixture = new Fixture();
            var tagNames = new List<string>
            {
                Template.ViewEngine.Name,
                Template.AppEngine.Name,
                fixture.Create<string>(),
                fixture.Create<string>()
            };

            var results = _galaxy.GetTemplatesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsNotEmpty(actual);
            Assert.That(actual, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetTemplatesByName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { _galaxy.GetTemplatesByName(null); });
        }

        [Test]
        public void GetInstanceByName_ValidTemplate_ReturnsValidObject()
        {
            const string tagName = Known.Instances.R31;

            var result = _galaxy.GetInstanceByName(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.Tagname);
        }

        [Test]
        public void GetInstanceByName_InvalidObject_ReturnsNull()
        {
            var fixture = new Fixture();
            var tagName = fixture.Create<string>();

            var result = _galaxy.GetInstanceByName(tagName);

            Assert.IsNull(result);
        }

        [Test]
        public void GetInstanceByName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { _galaxy.GetInstanceByName(null); });
        }

        [Test]
        public void GetInstancesByName_ValidTemplates_ReturnsExpectedCount()
        {
            var tagNames = new List<string>
            {
                Known.Instances.R31,
                Known.Instances.DrumConveyor
            };

            var results = _galaxy.GetInstancesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsNotEmpty(actual);
            Assert.That(actual, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetInstancesByName_InvalidObject_ReturnsIsEmpty()
        {
            var fixture = new Fixture();
            var tagNames = fixture.CreateMany<string>(4);

            var results = _galaxy.GetInstancesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetInstancesByName_SomeRealSomeFake_ReturnsExpectedCount()
        {
            var fixture = new Fixture();
            var tagNames = new List<string>
            {
                Known.Instances.R31,
                Known.Instances.DrumConveyor,
                fixture.Create<string>(),
                fixture.Create<string>()
            };

            var results = _galaxy.GetInstancesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsNotEmpty(actual);
            Assert.That(actual, Has.Count.EqualTo(2));
        }

        [Test]
        public void GetInstancesByName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { _galaxy.GetInstancesByName(null); });
        }

        [Test]
        public void GetBaseTemplates_WhenCalled_ReturnsExpectedCount()
        {
            var templates = _galaxy.GetBaseTemplates();

            //can get all but one _AutoImport...
            templates.count.Should().Be(Template.List.Count - 1);
        }

        [Test]
        public void GetBaseTemplate_SymbolTemplate_ShouldHaveTemplateName()
        {
            var template = _galaxy.GetBaseTemplate(Template.Symbol);

            template.Tagname.Should().Be(Template.Symbol.Name);
        }

        [Test]
        public void GetSymbolByName_KnownSymbolName_ShouldNotBeNull()
        {
            const string tagName = Known.Symbols.ProportionalValve;

            var result = _galaxy.GetSymbolByName(tagName);

            result.Should().NotBeNull();
            result.Tagname.Should().Be(tagName);
        }

        [Test]
        public void GetDerivedTemplates_WhenCalled_ReturnsKnownTemplates()
        {
            var templates = _galaxy.GetDerivedTemplates(Template.UserDefined.Name);

            var results = new List<string>();
            foreach (IgObject result in templates)
                results.Add(result.Tagname);

            Assert.That(results, Contains.Item(Known.Templates.ReactorSet.TagName));
            Assert.That(results, Contains.Item(Known.Templates.DrumConveyor));
        }

        [Test]
        public void GetDescendents_WhenCalled_ReturnsKnownObjects()
        {
            var descendents = _galaxy.GetDescendents(Template.UserDefined.Name);

            var results = new List<string>();
            foreach (IgObject result in descendents)
                results.Add(result.Tagname);

            Assert.That(results, Contains.Item(Known.Templates.ReactorSet.TagName));
            Assert.That(results, Contains.Item(Known.Templates.DrumConveyor));
            Assert.That(results, Contains.Item(Known.Instances.R31));
            Assert.That(results, Contains.Item(Known.Instances.DrumConveyor));
        }

        [Test]
        public void IsDerivedFrom_DerivedTemplate_ReturnsTrue()
        {
            var result = _galaxy.IsDescendentOf(Known.Templates.ReactorSet.TagName, Template.UserDefined.Name);
            Assert.True(result);
        }

        [Test]
        public void IsDerivedFrom_NonDerivedTemplate_ReturnsFalse()
        {
            var result = _galaxy.IsDescendentOf(Known.Templates.DrumConveyor.TagName, Known.Templates.ReactorSet.TagName);
            Assert.False(result);
        }

        [Test]
        public void IsDerivedFrom_DerivedInstance_ReturnsTrue()
        {
            var result = _galaxy.IsDescendentOf(Known.Instances.R31, Known.Templates.ReactorSet.TagName);
            Assert.True(result);
        }

        [Test]
        public void IsDerivedFrom_NonDerivedInstance_ReturnsFalse()
        {
            var result = _galaxy.IsDescendentOf(Known.Templates.ReactorSet.TagName, Known.Instances.DrumConveyor);
            Assert.False(result);
        }

        [Test]
        public void CreateObject_WhenCalled_CreatedExpectedObject()
        {
            var template = _galaxy.GetObjectByName(Template.UserDefined.Name);
            Assert.NotNull(template);

            _galaxy.CreateObject("$Test_Template", template.Tagname);

            var derived = _galaxy.GetObjectByName("$Test_Template");

            Assert.NotNull(derived);
            Assert.AreEqual("$Test_Template", derived.Tagname);
        }

        [Test]
        public void CreateSymbol_ValidTagName_ShouldReturnNewInstance()
        {
            var created = _galaxy.CreateSymbol("SomeSymbol");

            created.Tagname.Should().Be("SomeSymbol");
        }

        [Test]
        public void CreateAndDeleteMultipleNonContainedObjects()
        {
            var template = _galaxy.GetObjectByName(Template.UserDefined.Name);
            var created = new List<string>();
            var results = new List<string>();

            for (var i = 1; i <= 10; i++)
            {
                var tagName = $"$Test_Template_{i}";
                _galaxy.CreateTemplate(tagName, template.Tagname);
                created.Add(tagName);
            }

            for (var i = 1; i <= 10; i++)
            {
                var tagName = $"Test_Instance_{i}";
                _galaxy.CreateInstance(tagName, template.Tagname);
                created.Add(tagName);
            }

            var createdObjects = _galaxy.GetObjectsByName(created);

            foreach (IgObject result in createdObjects)
                results.Add(result.Tagname);

            Assert.That(results, Has.Count.EqualTo(20));

            foreach (IgObject createdObject in createdObjects)
                _galaxy.DeepDelete(createdObject.Tagname);


            createdObjects = _galaxy.GetObjectsByName(created);

            results.Clear();
            foreach (IgObject result in createdObjects)
                results.Add(result.Tagname);

            Assert.IsEmpty(results);
        }
        
        
        [Test]
        public void ExportObjects_KnownGraphic_FileShouldExist()
        {
            using var temp = new TempDirectory();
            var fileName = Path.Combine(temp.FullName, $"{Known.Symbols.React.TagName}.aaPKG");

            _galaxy.ExportObjects(Known.Templates.ReactorSet.TagName, fileName);

            File.Exists(fileName).Should().BeTrue();
        }

        [Test]
        public void ExportGraphic_WhenCalled_FileShouldExist()
        {
            using var temp = new TempDirectory();
            var fileName = Path.Combine(temp.FullName, $"{Known.Symbols.React.TagName}.xml");

            _galaxy.ExportGraphic(Known.Symbols.React.TagName, fileName);

            File.Exists(fileName).Should().BeTrue();
        }
        
        [Test]
        public void ImportGraphic_ValidXmlFile_ShouldExistAfterImport()
        {
            using var temp = new TempDirectory();
            var fileName = Path.Combine(temp.FullName, $"{Known.Symbols.React.TagName}.xml");

            _galaxy.ExportGraphic(Known.Symbols.React.TagName, fileName);

            File.Exists(fileName).Should().BeTrue();
        }
    }
}