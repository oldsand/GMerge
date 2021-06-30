using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IObjectRepository : IRepository<GObject>
    {
        GObject FindByTagName(string tagName);
        string GetTagName(int objectId);
        int GetObjectId(string tagName);
        GObject FindIncludeDescendants(string tagName);
        GObject FindIncludeFolder(string tagName);
        Task<IEnumerable<GObject>> GetDerivationHierarchy();
    }
}