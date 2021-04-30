using GalaxyMerge.Archestra;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Repositories;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class GalaxyArchiverTests
    {
        /*[Test]
        public void Archive_ValidTagName_CreateArchiveEntry()
        {
            var galaxyRepo = new GalaxyRepository("ButaneDev2014");
            var objectRepo = new ObjectRepository(ConnectionStringBuilder.BuildGalaxyConnection("ButaneDev2014"));
            var archiveRepo = new ArchiveRepository("ButaneDev2014");
            var archiver = new GalaxyArchiver(galaxyRepo, objectRepo, archiveRepo);
            
            archiver.Archive("$Test_Template");

            var result = archiveRepo.GetLatestEntry("$Test_Template");

            Assert.NotNull(result);
        }*/
    }
}