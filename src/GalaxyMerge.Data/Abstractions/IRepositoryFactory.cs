using System;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IRepositoryFactory
    {
        TRepository Create<T, TRepository>(string connectionString) 
            where TRepository : IRepository<T> where T : class;
    }
    
    
    public class ObjectFactory {
        public static TRepository Instance<T, TRepository>(string connectionString) 
            where TRepository : IRepository<T> where T : class
        {
            return (TRepository)Activator.CreateInstance(typeof(TRepository), connectionString);
        }
    }
}