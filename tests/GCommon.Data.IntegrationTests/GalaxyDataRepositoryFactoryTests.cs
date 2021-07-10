using GCommon.Data;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace GCommon.Data.IntegrationTests
{
    [TestFixture]
    public class GalaxyDataRepositoryFactoryTests
    {
        private SqlConnectionStringBuilder _connectionStringBuilder;

        [SetUp]
        public void Setup()
        {
            _connectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = "",
                InitialCatalog = "",
                IntegratedSecurity = true
            };
            
        }
        
        [Test]
        public void Create_ConnectionString_ReturnsNotNull()
        {
            var factory = new GalaxyDataProviderFactory();

            var repo = factory.Create(_connectionStringBuilder.ConnectionString);
            
            Assert.NotNull(repo);
        }
        
        [Test]
        public void Create_ConnectionStringBuilder_ReturnsNotNull()
        {
            var factory = new GalaxyDataProviderFactory();

            var repo = factory.Create(_connectionStringBuilder);
            
            Assert.NotNull(repo);
        }
    }
}