using GalaxyMerge.Test.Core;
using NUnit.Framework;

namespace GalaxyMerge.Archiving.Tests
{
    internal class ArchiveRepositoryTests : SqliteTestFixture<ArchiveContext>
    {
        [SetUp]
        public void Setup()
        {
            using var context = new ArchiveContext(ContextOptions);
            Create(context);
        }
        
        [TearDown]
        public void TearDown()
        {
            Dispose();
        }

        protected override void Seed()
        {
            using var context = new ArchiveContext(ContextOptions);
            
            //context.
        }
    }
}