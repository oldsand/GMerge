using GCommon.Data.Base;
using GCommon.Data.Entities;
using GCommon.Primitives;

namespace GCommon.Data.Abstractions
{
    public interface IChangeLogRepository : IReadOnlyRepository<ChangeLog>
    {
        ChangeLog GetLatest(int objectId);
        ChangeLog GetLatestByOperation(int objectId, Operation operation);
    }
}