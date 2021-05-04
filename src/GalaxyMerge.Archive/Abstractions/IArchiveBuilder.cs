namespace GalaxyMerge.Archive.Abstractions
{
    public interface IArchiveBuilder
    {
        void Build(ArchiveConfigurationBuilder configurationBuilder);
        void Configure(ArchiveConfigurationBuilder configurationBuilder);
    }
}