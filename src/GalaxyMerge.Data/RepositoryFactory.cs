using System;
using GalaxyMerge.Data.Abstractions;

namespace GalaxyMerge.Data
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public TRepository Create<T, TRepository>(string connectionString) 
            where T : class where TRepository : IRepository<T>
        {
            return (TRepository)Activator.CreateInstance(typeof(TRepository), connectionString);
        }
        
        public static TRepository Instance<T, TRepository>(string connectionString) 
            where TRepository : IRepository<T> where T : class
        {
            return (TRepository)Activator.CreateInstance(typeof(TRepository), connectionString);
        }
    }
}