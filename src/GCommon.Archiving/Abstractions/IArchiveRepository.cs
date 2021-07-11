using System;
using System.Threading.Tasks;
using GCommon.Primitives;
using GCommon.Archiving.Entities;

namespace GCommon.Archiving.Abstractions
{
    public interface IArchiveRepository : IDisposable
    {
        IEventSettingsRepository Events { get; }
        IInclusionSettingsRepository Inclusions { get; }
        IArchiveObjectRepository Objects { get; }
        IChangeLogInfoRepository ChangeLogs { get; }
        IQueueRepository Queue { get; }
        Archive GetArchiveInfo();
        Archive GetArchiveSettings();
        bool CanArchive(ArchiveObject archiveObject, Operation operation);
        bool HasChanges();
        int Save();
        Task<int> SaveAsync();
    }
}