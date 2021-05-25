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

        public UserProfile FindByName(string userName)
        {
            return Set.SingleOrDefault(x => x.UserName == userName);
        }
    }
}