namespace GalaxyMerge.Services.Abstractions
{
    public interface IArchiveProcessorFactory
    {
        IArchiveProcessor Create(string galaxyName);
    }
}