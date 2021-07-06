using System.Data.Common;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IDataRepositoryFactory
    {
        IDataRepository Create(string connectionString);
        IDataRepository Create(DbConnectionStringBuilder connectionStringBuilder);
    }
}