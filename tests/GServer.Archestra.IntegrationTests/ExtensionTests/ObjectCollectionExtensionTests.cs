using System;
using System.Collections.Generic;
using System.Linq;
using ArchestrA.GRAccess;
using GServer.Archestra.Extensions;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [TestFixture]
    public class ObjectCollectionExtensionTests
    {
        [Test]
        public void AsTemplates_WhenCalled_ReturnsCollectionOfITemplate()
        {
            var grAccess = new GRAccessAppClass();
            var galaxy = grAccess.QueryGalaxies(Environment.MachineName)["ButaneDev2014"];
            galaxy.Login(string.Empty, string.Empty);

            var tagNames = new List<string>
            {
                "$UserDefined", "$ViewEngine", "$AppEngine", "$InTouchViewApp", "$WinPlatform"
            };

            var templates = galaxy.GetTemplatesByName(tagNames).AsTemplates();

            Assert.IsInstanceOf<IEnumerable<ITemplate>>(templates);
            Assert.That(templates.ToList(), Has.Count.EqualTo(5));
        }
        
        [Test]
        public void AsInstances_WhenCalled_ReturnsCollectionOfITemplate()
        {
            var grAccess = new GRAccessAppClass();
            var galaxy = grAccess.QueryGalaxies(Environment.MachineName)["ButaneDev2014"];
            galaxy.Login(string.Empty, string.Empty);

            var tagNames = new List<string>
            {
                "SUN_GEN_Site_Area", "ETDEVGR1", "GR_Node", "FakeInstance"
            };

            var instances = galaxy.GetInstancesByName(tagNames).AsInstances();

            Assert.IsInstanceOf<IEnumerable<ITemplate>>(instances);
            Assert.That(instances.ToList(), Has.Count.EqualTo(3));
        }
    }
}