
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class ArchiveManualTesting
    {
        /*[Test]
        public void CreateAndSeedAnArchiveDatabaseForManualInspection()
        {
            var gr = new GalaxyRepository(Settings.CurrentTestGalaxy);
            gr.Login(WindowsIdentity.GetCurrent().Name);
            
            using var dataRepo = new DataRepository(Settings.CurrentTestHost, Settings.CurrentTestGalaxy);

            var config =
                ArchiveConfiguration.Default(gr.Name, gr.VersionNumber, gr.CdiVersion, gr.VersionString);
            var builder = new ArchiveBuilder();
            builder.Build(config);

            var archiver = new ArchiveProcessor(gr, dataRepo);
            archiver.Archive("$Test_Template", null, true);
            archiver.Archive("DatabaseAccess", null, true);
            archiver.Archive("$AnalogInput_v3", null, true);
            archiver.Archive("Generic_SubSystem_Injection", null, true);
            
            FileAssert.Exists(config.FileName);
        }*/
    }
}