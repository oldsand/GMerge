using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Services.Abstractions
{
    public interface IArchiver
    {
        void Archive(int objectId, bool force, int? changeLogId);
        void Archive(GObject gObject, bool force, int? changeLogId);
    }
}