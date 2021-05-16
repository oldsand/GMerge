using System.Linq;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Data.Repositories
{
    public class ChangeLogRepository : Repository<ChangeLog>, IChangeLogRepository
    {
        public ChangeLogRepository(string galaxyName) 
            : base(GalaxyContext.Create(ConnectionStringBuilder.BuildGalaxyConnection(galaxyName)))
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