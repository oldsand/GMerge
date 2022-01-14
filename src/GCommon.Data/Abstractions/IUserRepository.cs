using GCommon.Data.Base;
using GCommon.Data.Entities;

namespace GCommon.Data.Abstractions
{
    public interface IUserRepository : IReadOnlyRepository<UserProfile>
    {
        UserProfile FindByName(string userName);
    }
}