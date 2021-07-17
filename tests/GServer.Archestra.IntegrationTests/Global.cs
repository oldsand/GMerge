using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using ArchestrA.GRAccess;
using NUnit.Framework;

namespace GServer.Archestra.IntegrationTests
{
    [SetUpFixture]
    public class Global
    {
        private readonly bool RunGlobalSetup = true;
        private const string BaseGalaxyTemplate = "Base_Application_Server.cab";
        private const string ReactorGalaxyTemplate = "Reactor_Demo_Application_Server.cab";
        private const string KnownGalaxyName = "TestReactor";
        private const string TestGalaxyName = "GMergeTestGalaxy";
        
        public static string GalaxyName { get; private set; }
        public static string UserName => WindowsIdentity.GetCurrent().Name;

        [OneTimeSetUp]
        public void Setup()
        {
            if (!RunGlobalSetup)
            {
                GalaxyName = KnownGalaxyName;
                return;
            }

            GalaxyName = TestGalaxyName;
            
            Console.WriteLine($"Running one time setup for test project. Building test galaxy {GalaxyName}");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var access = new GRAccessAppClass();
            
            var templates = access.ListCreateGalaxyTemplates().ToList();
            var template = templates.SingleOrDefault(x => x == ReactorGalaxyTemplate) 
                           ?? templates.SingleOrDefault(x => x == BaseGalaxyTemplate);

            if (string.IsNullOrEmpty(template))
                throw new InvalidOperationException("Could not find template for test galaxy");

            access.CreateGalaxyFromTemplate(template, GalaxyName);
            
            stopWatch.Stop();
            Console.WriteLine($"Galaxy creation completed in {stopWatch.Elapsed}. Integration testing ready...");
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            if (!RunGlobalSetup) return;
            
            Console.WriteLine($"Running one time tear down for test project. Deleting test galaxy {GalaxyName}");
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var access = new GRAccessAppClass();
            
            access.DeleteGalaxy(GalaxyName);

            if (!access.CommandResult.Successful)
                throw new InvalidOperationException(
                    $"Could not delete galaxy {GalaxyName}. {access.CommandResult.ID}. {access.CommandResult.CustomMessage}");
            
            stopWatch.Stop();
            Console.WriteLine($"Galaxy deletion completed in {stopWatch.Elapsed}. Integration testing finished!");
        }
    }
}