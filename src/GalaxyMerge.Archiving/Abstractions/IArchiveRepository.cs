using System;
using System.Threading.Tasks;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Abstractions
{
    public interface IArchiveRepository : IDisposable
    {
        IEventSettingsRepository Events { get; }
        IInclusionSettingsRepository Inclusions { get; }
        IArchiveObjectRepository Objects { get; }
        IQueueRepository Queue { get; }
        Archive GetArchiveInfo();
        Archive GetArchiveSettings();
        bool CanArchive(ArchiveObject archiveObject, Operation operation);
        bool HasChanges();
        int Save();
        Task<int> SaveAsync();
    }
}