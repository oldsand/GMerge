using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Repositories;

namespace GCommon.Archiving
{
    public class ArchiveRepositoryFactory : IArchiveRepositoryFactory
    {
        public IArchiveRepository Create(string connectionString)
        {
            return new ArchiveRepository(connectionString);
        }
    }
}