using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IChangeLogRepository : IRepository<ChangeLog>
    {
        ChangeLog GetLastVersionChangeRecord(int objectId);
    }
}