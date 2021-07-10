using GCommon.Archiving.Entities;

namespace GServer.Services.Abstractions
{
    public interface IGalaxyArchiver
    {
        void Archive(ArchiveObject archiveObject);
    }
}