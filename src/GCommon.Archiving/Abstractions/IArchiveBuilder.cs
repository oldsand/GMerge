using GCommon.Primitives;

namespace GCommon.Archiving.Abstractions
{
    /// <summary>
    /// Interface that defines the api for building a new archive repository database. 
    /// </summary>
    public interface IArchiveBuilder
    {
        /// <summary>
        /// Builds an archive database using the provided archive configuration
        /// </summary>
        /// <param name="archive">The archive used to build the database</param>
        /// <param name="connectionString">Optionally provide the connection string for the database</param>
        void Build(Archive archive, string connectionString = null);
    }
}