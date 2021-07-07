using System;

namespace GalaxyMerge.Core.Utilities
{
    public interface IRepositoryCreator<out T>
    {
        T Create(string connectionString);
    }

    public class RepositoryCreator<T> : IRepositoryCreator<T>
    {
        public T Create(string connectionString)
        {
            return (T) Activator.CreateInstance(typeof(T), connectionString);
        }
    }
}