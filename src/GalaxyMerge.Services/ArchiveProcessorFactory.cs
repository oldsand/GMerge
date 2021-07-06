using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Services.Abstractions;

namespace GalaxyMerge.Services
{
    public class ArchiveProcessorFactory : IArchiveProcessorFactory
    {
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly IDataRepositoryFactory _dataRepositoryFactory;
        private readonly IArchiveRepositoryFactory _archiveRepositoryFactory;
        private readonly IArchiveQueue _archiveQueue;

        public ArchiveProcessorFactory(IGalaxyRegistry galaxyRegistry, 
            IDataRepositoryFactory dataRepositoryFactory,
            IArchiveRepositoryFactory archiveRepositoryFactory,
            IArchiveQueue archiveQueue)
        {
            _galaxyRegistry = galaxyRegistry;
            _dataRepositoryFactory = dataRepositoryFactory;
            _archiveRepositoryFactory = archiveRepositoryFactory;
            _archiveQueue = archiveQueue;
        }

        public IArchiveProcessor Create(string galaxyName)
        {
            return new ArchiveProcessor(galaxyName, _galaxyRegistry, _dataRepositoryFactory, 
                _archiveRepositoryFactory, _archiveQueue);
        }
    }
}