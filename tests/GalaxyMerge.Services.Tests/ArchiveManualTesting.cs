using System.Security.Principal;
using GalaxyMerge.Archestra;
using GalaxyMerge.Archive;
using GalaxyMerge.Testing;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class ArchiveManualTesting
    {
        [Test]
        public void CreateAndSeedAnArchiveDatabaseForManualInspection()
        {
            var gr = new GalaxyRepository(Settings.CurrentTestGalaxy);
            gr.Login(WindowsIdentity.GetCurrent().Name);

            var config =
                ArchiveConfigurationBuilder.Default(gr.Name, gr.VersionNumber, gr.CdiVersion, gr.VersionString);
            var builder = new ArchiveBuilder();
            builder.Build(config);

            var archiver = new ArchiveProcessor(gr);
            archiver.Archive("$Test_Template", null, true);
            archiver.Archive("DatabaseAccess", null, true);
            archiver.Archive("$AnalogInput_v3", null, true);
            archiver.Archive("Generic_SubSystem_Injection", null, true);
            
            FileAssert.Exists(config.FileName);
        }
    }
}