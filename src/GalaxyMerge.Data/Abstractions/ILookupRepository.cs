using System;
using System.Collections.Generic;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface ILookupRepository : IDisposable
    {
        IEnumerable<AncestorLookup> FindAncestors(int objectId, bool excludeSelf);
    }
}