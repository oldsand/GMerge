using System.Data.Common;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Data
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