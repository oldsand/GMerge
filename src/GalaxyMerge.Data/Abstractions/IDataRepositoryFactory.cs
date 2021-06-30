using System.Data.Common;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IDataRepositoryFactory
    {
        TRepository Create<T, TRepository>(DbConnectionStringBuilder connectionStringBuilder) 
            where TRepository : IRepository<T> where T : class;
        
        ILookupRepository CreateLookupRepository(DbConnectionStringBuilder connectionStringBuilder);
    }
}