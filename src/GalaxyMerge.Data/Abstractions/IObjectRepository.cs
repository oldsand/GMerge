using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IObjectRepository : IRepository<GObject>
    {
        string GetTagName(int objectId);
        int GetObjectId(string tagName);
        GObject FindIncludeDescendants(string tagName);
        Task<IEnumerable<GObject>> GetDerivationHierarchy();
    }
}