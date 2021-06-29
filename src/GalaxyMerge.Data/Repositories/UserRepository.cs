using System.Data.Common;
using System.Linq;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Repositories
{
    public class UserRepository : Repository<UserProfile>, IUserRepository
    {
        public UserRepository(string galaxyName) 
            : base(GalaxyContext.Create(DbStringBuilder.BuildGalaxy(galaxyName)))
        {
        }
        
        public UserRepository(string hostName, string galaxyName) 
            : base(GalaxyContext.Create(DbStringBuilder.BuildGalaxy(hostName, galaxyName)))
        {
        }
        
        public UserRepository(DbConnectionStringBuilder connectionStringBuilder) 
            : base(GalaxyContext.Create(connectionStringBuilder.ConnectionString))
        {
        }

        public UserProfile FindByName(string userName)
        {
            return Set.SingleOrDefault(x => x.UserName == userName);
        }
    }
}