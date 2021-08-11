using GCommon.Primitives;

namespace GServer.Services.Abstractions
{
    public interface IGalaxyArchiver
    {
        void Archive(ArchiveObject archiveObject);
    }
}