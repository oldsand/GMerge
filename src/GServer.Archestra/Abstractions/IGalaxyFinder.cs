using System.Collections.Generic;

namespace GServer.Archestra.Abstractions
{
    public interface IGalaxyFinder
    {
        bool Exists(string galaxyName);
        IEnumerable<string> Find();
    }
}