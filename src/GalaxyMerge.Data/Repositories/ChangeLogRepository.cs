using System.Linq;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    internal class ChangeLogRepository : Repository<ChangeLog>, IChangeLogRepository
    {
        internal ChangeLogRepository(DbContext context) : base (context)
        {
        }

        public ChangeLog GetLatest(int objectId)
        {
            return Set.OrderByDescending(x => x.ChangeDate)
                .FirstOrDefault(x => x.ObjectId == objectId);
        }

        public ChangeLog GetLatestByOperation(int objectId, Operation operation)
        {
            return Set.OrderByDescending(x => x.ChangeDate)
                .FirstOrDefault(x => x.ObjectId == objectId && x.OperationId == operation.Id);
        }
    }
}