using GalaxyMerge.Data.Abstractions;

namespace GalaxyMerge.Data
{
    public class DataRepositoryFactory<T> : IDataRepositoryFactory<T> where T : new()
    {
        public T Create(string connectionString)
        {
            return new T();
        }
    }
}