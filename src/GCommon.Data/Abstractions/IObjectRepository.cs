using System.Collections.Generic;
using System.Threading.Tasks;
using GCommon.Data.Base;
using GCommon.Data.Entities;

namespace GCommon.Data.Abstractions
{
    public interface IObjectRepository : IReadOnlyRepository<GalaxyObject>
    {
        GalaxyObject Find(int id);
        IEnumerable<GalaxyObject> Find(string tagName);
        string GetTagName(int objectId);
        int GetObjectId(string tagName);
        GalaxyObject FindIncludeTemplate(int objectId);
        GalaxyObject FindIncludeDescendants(string tagName);
        GalaxyObject FindIncludeFolder(string tagName);
        Task<IEnumerable<GalaxyObject>> GetDerivationHierarchy();
    }
}