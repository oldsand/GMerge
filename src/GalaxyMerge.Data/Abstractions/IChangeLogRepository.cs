using GalaxyMerge.Data.Entities;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IChangeLogRepository : IRepository<ChangeLog>
    {
        ChangeLog GetLatest(int objectId);
        ChangeLog GetLatestByOperation(int objectId, Operation operation);
    }
}