using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Services.Abstractions;

namespace GalaxyMerge.Services
{
    public class ArchiveProcessorFactory : IArchiveProcessorFactory
    {
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly IGalaxyDataProviderFactory _galaxyDataProviderFactory;
        private readonly IArchiveRepositoryFactory _archiveRepositoryFactory;
        private readonly IArchiveQueue _archiveQueue;

        public ArchiveProcessorFactory(IGalaxyRegistry galaxyRegistry, 
            IGalaxyDataProviderFactory galaxyDataProviderFactory,
            IArchiveRepositoryFactory archiveRepositoryFactory,
            IArchiveQueue archiveQueue)
        {
            _galaxyRegistry = galaxyRegistry;
            _galaxyDataProviderFactory = galaxyDataProviderFactory;
            _archiveRepositoryFactory = archiveRepositoryFactory;
            _archiveQueue = archiveQueue;
        }

        public IArchiveProcessor Create(string galaxyName)
        {
            return new ArchiveProcessor(galaxyName, _galaxyRegistry, _galaxyDataProviderFactory, 
                _archiveRepositoryFactory, _archiveQueue);
        }
    }
}