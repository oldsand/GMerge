using System.Data.Common;
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
            : base(GalaxyContext.Create(DbStringBuilder.BuildGalaxy(galaxyName)))
        {
        }
        
        public ChangeLogRepository(string hostName, string galaxyName) 
            : base(GalaxyContext.Create(DbStringBuilder.BuildGalaxy(hostName, galaxyName)))
        {
        }
        
        public ChangeLogRepository(DbConnectionStringBuilder connectionStringBuilder) 
            : base(GalaxyContext.Create(connectionStringBuilder.ConnectionString))
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