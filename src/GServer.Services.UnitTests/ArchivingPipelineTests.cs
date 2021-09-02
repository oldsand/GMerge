using System;
using GCommon.Archiving.Abstractions;
using GCommon.Data.Abstractions;
using GCommon.Data.Entities;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using GServer.Archestra.Abstractions;
using Moq;
using NUnit.Framework;

namespace GServer.Services.UnitTests
{
    [TestFixture]
    public class ArchivingPipelineTests
    {
        private ChangeLog _changeLog;
        private GalaxyObject _galaxyObject;
        private ArchiveObject _archiveObject;
        private IGalaxyRepository _galaxy;
        private IArchiveRepositoryFactory _archiveFactory;
        private IGalaxyDataProviderFactory _providerFactory;
        private Mock<IGalaxyRepository> _mockGalaxy;
        private Mock<IArchiveRepository> _mockArchive;
        private Mock<IGalaxyDataProvider> _mockProvider;
        private IObjectRepository _objectRepositoryMock;

        /*[SetUp]
        public void Setup()
        {
            _changeLog = new ChangeLog
            {
                ChangeLogId = 1234,
                ChangeDate = DateTime.Today,
                Operation = Operation.CreateDerivedTemplate,
                Comment = "SomeComments"
            };
            
            _galaxyObject = new GalaxyObject
            {
                ObjectId = 4254,
                TagName = "TestObject",
                ConfigVersion = 1,
                Template = Template.UserDefined
            };

            _archiveObject = new ArchiveObject(_galaxyObject.ObjectId, _galaxyObject.TagName,
                _galaxyObject.ConfigVersion, _galaxyObject.Template);
            
            _mockGalaxy = new Mock<IGalaxyRepository>();
            _mockArchive = new Mock<IArchiveRepository>();
            _mockProvider = new Mock<IGalaxyDataProvider>();

            _mockGalaxy.Setup(x => x.GetGraphic(It.IsAny<string>())).Returns<string>(s => new ArchestraGraphic(s));
            _mockGalaxy.Setup(x => x.GetObject(It.IsAny<string>())).Returns(new ArchestraObject());
            
            _archiveObjectRepositoryMock = Mock.Of<IArchiveObjectRepository>();
            _mockArchive.Setup(x => x.Objects).Returns(_archiveObjectRepositoryMock);
            _mockArchive.Setup(x => x.IsArchivable(It.IsAny<ArchiveObject>())).Returns(true);
            
            _objectRepositoryMock = Mock.Of<IObjectRepository>();
            Mock.Get(_objectRepositoryMock).Setup(x => x.Find(It.IsAny<int>())).Returns(_galaxyObject);
            
            _mockProvider.Setup(x => x.Objects).Returns(_objectRepositoryMock);
            
            var archiveFactoryMock = new Mock<IArchiveRepositoryFactory>();
            archiveFactoryMock.Setup(m => m.Create(It.IsAny<string>()))
                .Returns(_mockArchive.Object);
            _archiveFactory = archiveFactoryMock.Object;
            
            var providerFactoryMock = new Mock<IGalaxyDataProviderFactory>();
            providerFactoryMock.Setup(m => m.Create(It.IsAny<string>()))
                .Returns(_mockProvider.Object);
            _providerFactory = providerFactoryMock.Object;

            _galaxy = _mockGalaxy.Object;
        }


        [Test]
        public void Enqueue_ValidChangeLog_CallsArchiveEnqueueWithChangeLogId()
        {
            var pipeline = new ArchivingPipeline(_galaxy, _providerFactory, _archiveFactory);
            pipeline.Start();

            pipeline.Enqueue(_changeLog);

            pipeline.Complete();
            pipeline.Completion.Wait();
            
            //Mock.Get(_queuedLogRepositoryMock).Verify(x => x.Enqueue(It.IsAny<QueuedLog>()), Times.Once);
        }*/
    }
}