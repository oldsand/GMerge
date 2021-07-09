using GalaxyMerge.Data.Base;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IChangeLogReadOnlyRepository : IReadOnlyRepository<ChangeLog>
    {
        ChangeLog GetLatest(int objectId);
        ChangeLog GetLatestByOperation(int objectId, Operation operation);
    }
}