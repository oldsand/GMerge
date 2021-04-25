using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IUserRepository : IRepository<UserProfile>
    {
        UserProfile FindByName(string userName);
    }
}