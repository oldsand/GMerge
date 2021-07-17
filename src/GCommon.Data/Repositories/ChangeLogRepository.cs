using System.Linq;
using GCommon.Data.Abstractions;
using GCommon.Data.Base;
using GCommon.Data.Entities;
using GCommon.Primitives;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Data.Repositories
{
    internal class ChangeLogRepository : ReadOnlyRepository<ChangeLog>, IChangeLogRepository
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
                .FirstOrDefault(x => x.ObjectId == objectId && x.Operation == operation);
        }
    }
}