namespace GCommon.Archiving.Abstractions
{
    public interface IArchiveRepositoryFactory
    {
        IArchiveRepository Create(string connectionString);
    }
}