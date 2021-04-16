namespace GalaxyMerge.IO.Abstractions
{
    public interface IGalaxyFileFactory
    {
        IGalaxyFile FromFile(string fileName);
    }
}