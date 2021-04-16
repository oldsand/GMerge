using System;
using System.Collections.Generic;

namespace GalaxyMerge.IO.Abstractions
{
    public interface IPkgFile : IGalaxyFile
    {
        IManifest ManifestFile { get; }
        IEnumerable<byte[]> ReadAllObjects();
    }
}