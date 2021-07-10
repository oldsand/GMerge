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
        /// <param name="configuration">The configuration used to build the database</param>
        void Build(ArchiveConfiguration configuration);
    }
}