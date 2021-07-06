using GalaxyMerge.Archiving.Abstractions;

namespace GalaxyMerge.Archiving
{
    public class ArchiveRepositoryFactory : IArchiveRepositoryFactory
    {
        public IArchiveRepository Create(string connectionString)
        {
            return null;
        }
        
        public IArchiveRepository CreateObjectRepository(string connectionString)
        {
            return null;
        }
        
        public IQueueRepository CreateQueueRepository(string connectionString)
        {
            return null;
        }
    }
}