using System;
using System.Collections.Generic;
using System.Linq;
using ArchestrA.GRAccess;
using GalaxyMerge.Archestra.Extensions;
using NUnit.Framework;

namespace GalaxyMerge.Archestra.Tests
{
    [TestFixture]
    public class GalaxyExtensionTests
    {
        private static IGalaxy _galaxy;

        [SetUp]
        public void Setup()
        {
            var grAccess = new GRAccessAppClass();
            _galaxy = grAccess.QueryGalaxies(Environment.MachineName)[Settings.CurrentTestGalaxy];
            _galaxy.Login(string.Empty, string.Empty);
        }

        [Test]
        public void CheckoutObjects_ValidTagNames_AllObjectsReturnCheckedOut()
        {
            var tagList = new List<string> {"$Test_Template", "Test_Template_001"};

            var objects = _galaxy.GetObjectsByName(tagList);
            objects.CheckOut();

            foreach (IgObject gObject in objects)
                Assert.True(gObject.IsCheckedOut());

            _galaxy.DeepCheckIn(tagList);

            foreach (IgObject gObject in objects)
                Assert.False(gObject.IsCheckedOut());
        }

        [Test]
        [TestCase("$Test_Template")]
        [TestCase("SUN_GEN_Site_Data")]
        [TestCase("SUN_GEN_Site_Area")]
        [TestCase("ETDEVTEST1")]
        [TestCase("$UserDefined")]
        public void GetObjectByName_ValidObject_ReturnsInstanceWithTagName(string tagName)
        {
            var result = _galaxy.GetObjectByName(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.Tagname);
        }

        [Test]
        [TestCase("Test_Template")]
        [TestCase("FakeObject")]
        [TestCase("$UserUndefined")]
        public void GetObjectByName_InvalidObject_ReturnsNull(string tagName)
        {
            var result = _galaxy.GetObjectByName(tagName);

            Assert.IsNull(result);
        }

        [Test]
        public void GetObjectByName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { _galaxy.GetObjectByName(null); });
        }

        [Test]
        [TestCase("$ViewEngine")]
        [TestCase("$UserDefined")]
        [TestCase("$InTouchViewApp")]
        [TestCase("$AppEngine")]
        public void GetTemplateByName_ValidTemplate_ReturnsValidObject(string tagName)
        {
            var result = _galaxy.GetTemplateByName(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.Tagname);
        }

        [Test]
        [TestCase("$Test_Templates")]
        [TestCase("$FakeObject")]
        [TestCase("$UserUndefined")]
        public void GetTemplateByName_InvalidObject_ReturnsNull(string tagName)
        {
            var result = _galaxy.GetTemplateByName(tagName);

            Assert.IsNull(result);
        }

        [Test]
        public void GetTemplateByName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { _galaxy.GetTemplateByName(null); });
        }

        [Test]
        public void GetTemplatesByName_ValidTemplates_ReturnsValidObjects()
        {
            var tagNames = new List<string>
            {
                "$ViewEngine", "$AppEngine", "$WinPlatform", "$InTouchViewApp"
            };

            var results = _galaxy.GetTemplatesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsNotEmpty(actual);
            Assert.That(actual, Has.Count.EqualTo(4));
        }

        [Test]
        public void GetTemplatesByName_InvalidObject_ReturnsNull()
        {
            var tagNames = new List<string>
            {
                "$Test_Templates", "$FakeObject", "$UserUndefined", "Something"
            };

            var results = _galaxy.GetTemplatesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetTemplatesByName_SomeRealSomeFake_ReturnsInstanceWithTagName()
        {
            var tagNames = new List<string>
            {
                "$Test_Templates", "$FakeObject", "$UserDefined", "Something"
            };

            var results = _galaxy.GetTemplatesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsNotEmpty(actual);
            Assert.That(actual, Has.Count.EqualTo(1));
            Assert.IsTrue(actual.Any(t => t == "$UserDefined"));
        }

        [Test]
        public void GetTemplatesByName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { _galaxy.GetTemplatesByName(null); });
        }

        [Test]
        [TestCase("SUN_GEN_Site_Data")]
        [TestCase("SUN_GEN_Site_Area")]
        [TestCase("GR_Node")]
        [TestCase("ETDEVTEST1")]
        public void GetInstanceByName_ValidTemplate_ReturnsValidObject(string tagName)
        {
            var result = _galaxy.GetInstanceByName(tagName);

            Assert.NotNull(result);
            Assert.AreEqual(tagName, result.Tagname);
        }

        [Test]
        [TestCase("DoesntMatter")]
        [TestCase("FakeObject")]
        [TestCase("$UserUndefined")]
        public void GetInstanceByName_InvalidObject_ReturnsNull(string tagName)
        {
            var result = _galaxy.GetInstanceByName(tagName);

            Assert.IsNull(result);
        }

        [Test]
        public void GetInstanceByName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { _galaxy.GetInstanceByName(null); });
        }

        [Test]
        public void GetInstancesByName_ValidTemplates_ReturnsValidObjects()
        {
            var tagNames = new List<string>
            {
                "SUN_GEN_Site_Data", "ETDEVTEST1", "GR_Node"
            };

            var results = _galaxy.GetInstancesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsNotEmpty(actual);
            Assert.That(actual, Has.Count.EqualTo(3));
        }

        [Test]
        public void GetInstancesByName_InvalidObject_ReturnsNull()
        {
            var tagNames = new List<string>
            {
                "Test_Fake", "FakeObject", "Something"
            };

            var results = _galaxy.GetInstancesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsEmpty(actual);
        }

        [Test]
        public void GetInstancesByName_SomeRealSomeFake_ReturnsInstanceWithTagName()
        {
            var tagNames = new List<string>
            {
                "SUN_GEN_Site_Data", "FakeObject", "", "Something"
            };

            var results = _galaxy.GetInstancesByName(tagNames);

            var actual = new List<string>();
            foreach (IgObject result in results)
                actual.Add(result.Tagname);

            Assert.IsNotEmpty(actual);
            Assert.That(actual, Has.Count.EqualTo(1));
            Assert.IsTrue(actual.Any(t => t == "SUN_GEN_Site_Data"));
        }

        [Test]
        public void GetInstancesByName_Null_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => { _galaxy.GetInstancesByName(null); });
        }

        [Test]
        public void GetDerivedTemplates_WhenCalled_ReturnsTemplates()
        {
            var templates = _galaxy.GetDerivedTemplates("$UserDefined");
            foreach (IgObject template in templates)
            {
                var galaxyObject = template.AsGalaxyObject();
                Assert.NotNull(galaxyObject);
            }
        }

        [Test]
        public void GetDescendents_WhenCalled_ReturnsDerivedObjects()
        {
            var results = _galaxy.GetDescendents("$UserDefined");

            foreach (IgObject result in results)
            {
                var galaxyObject = result.AsGalaxyObject();
                Assert.NotNull(galaxyObject);
            }
        }

        [Test]
        public void AssignToHost_AreaObject_ReturnsExpectedHost()
        {
            var area = new[] {"Area_002"};
            _galaxy.AssignToHost(area, "ObjectEngine_001");
            var result = _galaxy.GetObjectByName("Area_002");
            Assert.AreEqual("ObjectEngine_001", result.Host);
        }
        
        [Test]
        public void AssignToArea_ApplicationObject_ReturnsExpectedArea()
        {
            var tagNames = new[] {"AlarmExtension_001", "AlarmExtension_002", "AnalogExtensions_001"};
            _galaxy.AssignToArea(tagNames, "Area_002");
            var results = _galaxy.GetObjectsByName(tagNames);
            foreach (IgObject gObject in results)
                Assert.AreEqual("Area_002", gObject.Area);
        }

        [Test]
        public void IsDerivedFrom_ObjectDerivedFromTemplate_ReturnsTrue()
        {
            var result = _galaxy.ObjectIsDescendentOf("$Test_Template", "$UserDefined");
            Assert.True(result);
        }
        
        [Test]
        public void CreateObject_WhenCalled_CreatedExpectedObject()
        {
            var template = _galaxy.GetObjectByName("$Test_Template");
            Assert.NotNull(template);

            _galaxy.CreateObject("$DerivedTemplate001", template.Tagname);

            var derived = _galaxy.GetObjectByName("$DerivedTemplate001");
            
            Assert.NotNull(derived);
            Assert.AreEqual("$DerivedTemplate001", derived.Tagname);
        }
        
        [Test]
        public void CreateAndDeleteMultipleNonContainedObjects()
        {
            var template = _galaxy.GetObjectByName("$Test_Template");
            var created = new List<string>();

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

            Assert.That(createdObjects, Has.Count.EqualTo(20));

            foreach (IgObject createdObject in createdObjects)
                _galaxy.DeepDelete(createdObject.Tagname);


            createdObjects = _galaxy.GetObjectsByName(created);

            Assert.IsEmpty(createdObjects);
        }

        [Test]
        public void RecursiveDelete_WhenCalled_DeleteAll()
        {
            _galaxy.DeepDelete("$LimitAlarms");

            var result = _galaxy.GetObjectByName("$LimitAlarms");
            
            Assert.Null(result);
        }
    }
}