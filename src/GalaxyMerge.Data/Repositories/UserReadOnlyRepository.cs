using System.Linq;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Base;
using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    internal class UserReadOnlyRepository : ReadOnlyRepository<UserProfile>, IUserReadOnlyRepository
    {
        internal UserReadOnlyRepository(DbContext context) : base(context)
        {
        }

        public UserProfile FindByName(string userName)
        {
            return Set.SingleOrDefault(x => x.UserName == userName);
        }
    }
}