
using System.Data.Common;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Data.Abstractions
{
    public class DataRepositoryFactory : IDataRepositoryFactory
    {
        public IDataRepository Create(string connectionString)
        {
            return new DataRepository(connectionString);
        }

        public IDataRepository Create(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new DataRepository(connectionStringBuilder.ConnectionString);
        }
    }
}