using System.Data.Common;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IGalaxyDataRepositoryFactory
    {
        IGalaxyDataRepository Create(string connectionString);
        IGalaxyDataRepository Create(DbConnectionStringBuilder connectionStringBuilder);
    }
}