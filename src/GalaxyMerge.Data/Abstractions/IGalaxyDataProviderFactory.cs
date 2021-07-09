using System.Data.Common;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IGalaxyDataProviderFactory
    {
        IGalaxyDataProvider Create(string connectionString);
        IGalaxyDataProvider Create(DbConnectionStringBuilder connectionStringBuilder);
    }
}