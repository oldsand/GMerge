using System.Collections.Generic;
using System.Threading.Tasks;
using GCommon.Data.Base;
using GCommon.Data.Entities;

namespace GCommon.Data.Abstractions
{
    public interface IObjectRepository : IReadOnlyRepository<GObject>
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