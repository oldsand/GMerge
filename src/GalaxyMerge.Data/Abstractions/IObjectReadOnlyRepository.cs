using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyMerge.Data.Base;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IObjectReadOnlyRepository : IReadOnlyRepository<GObject>
    {
        GObject Find(int id);
        IEnumerable<GObject> Find(string tagName);
        string GetTagName(int objectId);
        int GetObjectId(string tagName);
        GObject FindIncludeTemplate(int objectId);
        GObject FindIncludeDescendants(string tagName);
        GObject FindIncludeFolder(string tagName);
        Task<IEnumerable<GObject>> GetDerivationHierarchy();
    }
}