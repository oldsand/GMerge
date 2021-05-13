using System;
using System.Collections.Generic;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface ILookupRepository : IDisposable
    {
        IEnumerable<ObjectLookup> FindAncestors(int objectId, bool excludeSelf = true);
        IEnumerable<ObjectLookup> FindDescendants(int objectId, bool includeInstances = false);
        string GetFolderPath(int objectId, int folderType = 2);
    }
}