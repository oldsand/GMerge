using System;
using System.Data.Common;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Data.Abstractions
{
    public class DataRepositoryFactory : IDataRepositoryFactory 
    {
        public TRepository Create<T, TRepository>(DbConnectionStringBuilder connectionStringBuilder) 
            where T : class where TRepository : IRepository<T>
        {
            return (TRepository) Activator.CreateInstance(typeof(TRepository), connectionStringBuilder);
        }

        public ILookupRepository CreateLookupRepository(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new LookupRepository(connectionStringBuilder);
        }
    }
}