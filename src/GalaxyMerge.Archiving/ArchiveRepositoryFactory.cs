using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Archiving.Repositories;

namespace GalaxyMerge.Archiving
{
    public class ArchiveRepositoryFactory : IArchiveRepositoryFactory
    {
        public IArchiveRepository Create(string connectionString)
        {
            return new ArchiveRepository(connectionString);
        }
    }
}