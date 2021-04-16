using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IObjectRepository : IRepository<GObject>
    {
        GObject FindById(int objectId);
        GObject FindByIdIncludeTemplate(int objectId);
        GObject FindByTagName(string tagName);
        GObject FindIncludeDerivations(string tagName);
    }
}