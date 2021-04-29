using System.Linq;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Helpers;

namespace GalaxyMerge.Data.Repositories
{
    public class ChangeLogRepository : Repository<ChangeLog>, IChangeLogRepository
    {
        public ChangeLogRepository(string connectionString) : base(ContextCreator.Create(connectionString))
        {
        }

        public ChangeLog GetLastVersionChangeRecord(int objectId)
        {
            return Set.OrderByDescending(x => x.ChangeDate)
                .FirstOrDefault(x => x.ObjectId == objectId && x.OperationId == 0);
        }
    }
}