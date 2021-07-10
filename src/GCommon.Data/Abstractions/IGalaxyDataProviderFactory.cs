using System.Data.Common;

namespace GCommon.Data.Abstractions
{
    public interface IGalaxyDataProviderFactory
    {
        IGalaxyDataProvider Create(string connectionString);
        IGalaxyDataProvider Create(DbConnectionStringBuilder connectionStringBuilder);
    }
}