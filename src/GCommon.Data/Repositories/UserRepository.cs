using System.Linq;
using GCommon.Data.Abstractions;
using GCommon.Data.Base;
using GCommon.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Data.Repositories
{
    internal class UserRepository : ReadOnlyRepository<UserProfile>, IUserRepository
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