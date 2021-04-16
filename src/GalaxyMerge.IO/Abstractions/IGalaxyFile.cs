using System;

namespace GalaxyMerge.IO.Abstractions
{
    public interface IGalaxyFile
    {
        string Name { get; }
        string Extension { get; }
        long Size { get; }
        DateTime CreatedOn { get; }
        DateTime UpdatedOn { get; }
        IGalaxyFile Load(string fileName);
        void Save(string fileName);
        byte[] GetBinaryData(string tagName);
    }
}