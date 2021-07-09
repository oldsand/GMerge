using GalaxyMerge.Data.Base;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IUserReadOnlyRepository : IReadOnlyRepository<UserProfile>
    {
        UserProfile FindByName(string userName);
    }
}