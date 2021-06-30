
using System.Data.Common;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Data.Abstractions
{
    public class GalaxyDataRepositoryFactory : IGalaxyDataRepositoryFactory
    {
        public IGalaxyDataRepository Create(string connectionString)
        {
            return new GalaxyDataRepository(connectionString);
        }

        public IGalaxyDataRepository Create(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new GalaxyDataRepository(connectionStringBuilder.ConnectionString);
        }
    }
}