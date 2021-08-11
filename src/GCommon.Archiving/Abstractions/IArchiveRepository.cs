using System;
using System.Threading.Tasks;
using GCommon.Primitives;

namespace GCommon.Archiving.Abstractions
{
    public interface IArchiveRepository : IDisposable
    {
        IEventSettingsRepository Events { get; }
        IInclusionSettingsRepository Inclusions { get; }
        IArchiveObjectRepository Objects { get; }
        IArchiveLogRepository Logs { get; }
        IQueuedLogRepository Queue { get; }
        Archive GetArchiveInfo();
        Archive GetArchiveSettings();
        bool IsArchivable(ArchiveObject archiveObject);
        bool HasChanges();
        int Save();
        Task<int> SaveAsync();
    }
}