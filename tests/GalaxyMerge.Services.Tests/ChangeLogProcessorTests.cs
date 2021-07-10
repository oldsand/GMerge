using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;
using GCommon.Data.Abstractions;
using GCommon.Data.Entities;
using GCommon.Primitives;
using GServer.Services.Processors;
using Moq;
using NUnit.Framework;

namespace GalaxyMerge.Services.Tests
{
    [TestFixture]
    public class ChangeLogProcessorTests
    {
        private GObject _testObject;

        [SetUp]
        public void Setup()
        {
            _testObject = new GObject
            {
                ObjectId = 100,
                TagName = "SomeTagName",
                ConfigVersion = 12,
                TemplateId = Template.UserDefined.Id
            };
        }
        
        [Test]
        public void Enqueue_ValidArchiveObject_VerifyExpectedCalls()
        {
            var mockProvider = new Mock<IGalaxyDataProvider>();
            mockProvider.Setup(p => p.Objects.Find(It.IsAny<int>()))
                .Returns(_testObject);

            var mockRepository = new Mock<IArchiveRepository>();
            mockRepository.Setup(r => r.CanArchive(It.IsAny<ArchiveObject>(), It.IsAny<Operation>())).Returns(true).Verifiable();
            mockRepository.Setup(r => r.Objects.Upsert(It.IsAny<ArchiveObject>())).Verifiable();
            mockRepository.Setup(r => r.Queue.Add(It.IsAny<QueuedEntry>())).Verifiable();
            mockRepository.Setup(r => r.Save()).Verifiable();

            var mockProviderFactory = new Mock<IGalaxyDataProviderFactory>();
            mockProviderFactory.Setup(f => f.Create(It.IsAny<string>())).Returns(mockProvider.Object);
            
            var mockRepositoryFactory = new Mock<IArchiveRepositoryFactory>();
            mockRepositoryFactory.Setup(f => f.Create(It.IsAny<string>())).Returns(mockRepository.Object);

            var processor = new ChangeLogProcessor(mockProviderFactory.Object, mockRepositoryFactory.Object);
            
            processor.Enqueue(new ChangeLog());
        }
        
        [Test]
        public void Enqueue_InvalidArchiveObject_VerifyExpectedCalls()
        {
            var mockProvider = new Mock<IGalaxyDataProvider>();
            mockProvider.Setup(p => p.Objects.Find(It.IsAny<int>()))
                .Returns(_testObject);

            var mockRepository = new Mock<IArchiveRepository>();
            mockRepository.Setup(r => r.CanArchive(It.IsAny<ArchiveObject>(), It.IsAny<Operation>())).Returns(false).Verifiable();
            mockRepository.Setup(r => r.Objects.Upsert(It.IsAny<ArchiveObject>())).Verifiable();
            mockRepository.Setup(r => r.Queue.Add(It.IsAny<QueuedEntry>())).Verifiable();
            mockRepository.Setup(r => r.Save()).Verifiable();

            var mockProviderFactory = new Mock<IGalaxyDataProviderFactory>();
            mockProviderFactory.Setup(f => f.Create(It.IsAny<string>())).Returns(mockProvider.Object);
            
            var mockRepositoryFactory = new Mock<IArchiveRepositoryFactory>();
            mockRepositoryFactory.Setup(f => f.Create(It.IsAny<string>())).Returns(mockRepository.Object);

            var processor = new ChangeLogProcessor(mockProviderFactory.Object, mockRepositoryFactory.Object);
            
            processor.Enqueue(new ChangeLog());
        }
    }
}