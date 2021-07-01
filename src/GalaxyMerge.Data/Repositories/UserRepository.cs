using System.Data.Common;
using System.Linq;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    internal class UserRepository : Repository<UserProfile>, IUserRepository
    {
        internal UserRepository(DbContext context) : base(context)
        {
        }

        public UserProfile FindByName(string userName)
        {
            return Set.SingleOrDefault(x => x.UserName == userName);
        }
    }
}