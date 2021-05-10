using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IChangeLogRepository : IRepository<ChangeLog>
    {
        ChangeLog GetLatest(int objectId);
        ChangeLog GetLatestByOperation(int objectId, Operation operation);
    }
}