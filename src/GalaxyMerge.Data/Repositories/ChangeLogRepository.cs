using System.Linq;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Helpers;

namespace GalaxyMerge.Data.Repositories
{
    public class ChangeLogRepository : Repository<ChangeLog>, IChangeLogRepository
    {
        public ChangeLogRepository(string galaxyName) 
            : base(ContextCreator.Create(ConnectionStringBuilder.BuildGalaxyConnection(galaxyName)))
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