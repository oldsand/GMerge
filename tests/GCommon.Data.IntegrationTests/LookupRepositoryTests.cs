using GTest.Core;
using GCommon.Data;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace GCommon.Data.IntegrationTests
{
    [TestFixture]
    public class LookupRepositoryTests
    {
        private const string HostName = Settings.CurrentTestHost;
        private const string DatabaseName = Settings.CurrentTestGalaxy;
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            _connectionString = new SqlConnectionStringBuilder
            {
                DataSource = HostName, 
                InitialCatalog = DatabaseName, 
                IntegratedSecurity = true
            }.ConnectionString;
        }
        
        [Test]
        [TestCase(13317)] //$Test_Template in ButaneDev2014
        [TestCase(829)] //$AnalogInput_v3 in ButaneDev2014
        public void FindAncestors_ValidObjectId_ReturnsExpectedAncestors(int objectId)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var ancestors = repo.Lookup.FindAncestors(objectId);
            
            Assert.IsNotEmpty(ancestors);
        }

        [Test]
        public void FindAncestors_InvalidId_ReturnsEmptyList()
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var ancestors = repo.Lookup.FindAncestors(0);

            Assert.IsEmpty(ancestors);
        }
        
        [Test]
        [TestCase(13317)] //$Test_Template in ButaneDev2014
        [TestCase(829)] //$AnalogInput_v3 in ButaneDev2014
        public void FindDescendants_ValidObjectId_ReturnsExpectedAncestors(int objectId)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var descendants = repo.Lookup.FindDescendants(objectId);
            
            Assert.IsNotEmpty(descendants);
        }

        [Test]
        public void FindDescendants_InvalidId_ReturnsEmptyList()
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var descendants = repo.Lookup.FindDescendants(0);

            Assert.IsEmpty(descendants);
        }

        [Test]
        [TestCase(170, "ArchestrA Symbol Library\\Fun Stuff")]
        [TestCase(38107, "_InTouch_Demo_\\Shared primitives\\Alarms")]
        public void GetFolderPath_ValidId_ReturnsExpectedFolderPath(int objectId, string expectedPath)
        {
            var repo = new GalaxyDataProvider(_connectionString);

            var path = repo.Lookup.GetFolderPath(objectId);
            
            Assert.AreEqual(expectedPath, path);
        }
    }
}