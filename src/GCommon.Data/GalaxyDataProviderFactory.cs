using System.Data.Common;
using GCommon.Data.Repositories;
using GCommon.Data.Abstractions;

namespace GCommon.Data
{
    public class GalaxyDataProviderFactory : IGalaxyDataProviderFactory
    {
        public IGalaxyDataProvider Create(string connectionString)
        {
            return new GalaxyDataProvider(connectionString);
        }

        public IGalaxyDataProvider Create(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new GalaxyDataProvider(connectionStringBuilder.ConnectionString);
        }
    }
}