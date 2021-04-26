namespace GalaxyMerge.Data.Abstractions
{
    public interface IDataRepositoryFactory<out T>
    {
        T Create(string connectionString);
    }
}