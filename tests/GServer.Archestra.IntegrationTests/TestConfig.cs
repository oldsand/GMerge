using System;
using System.Linq;
using System.Security.Principal;
using ArchestrA.GRAccess;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [SetUpFixture]
    public class TestConfig
    {
        // ReSharper disable once ConvertToConstant.Local
        // This is a configuration bit. Determines if the setup create and deletes a test galaxy before running all tests.
        private static readonly bool RunSetup = false;
        private const string BaseGalaxyTemplate = "Base_Application_Server.cab";
        private const string ReactorGalaxyTemplate = "Reactor_Demo_Application_Server.cab";
        private const string KnownGalaxyName = "TestReactor";
        private const string TestGalaxyName = "GMergeTestGalaxy";

        public static string GalaxyName { get; private set; }
        public static string UserName => WindowsIdentity.GetCurrent().Name;

        [OneTimeSetUp]
        public void Setup()
        {
            if (!RunSetup)
            {
                GalaxyName = KnownGalaxyName;
                return;
            }
            
            GalaxyName = TestGalaxyName;
            
            var access = new GRAccessAppClass();

            var templates = access.ListCreateGalaxyTemplates().ToList();
            var template = templates.SingleOrDefault(x => x == ReactorGalaxyTemplate)
                           ?? templates.SingleOrDefault(x => x == BaseGalaxyTemplate);

            if (string.IsNullOrEmpty(template))
                throw new InvalidOperationException("Could not find template for test galaxy");

            access.CreateGalaxyFromTemplate(template, GalaxyName);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (!RunSetup) return;

            var access = new GRAccessAppClass();

            access.DeleteGalaxy(GalaxyName);

            if (!access.CommandResult.Successful)
                throw new InvalidOperationException(
                    $"Could not delete galaxy {GalaxyName}. {access.CommandResult.ID}. {access.CommandResult.CustomMessage}");
        }
    }
}