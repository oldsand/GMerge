using System;
using System.Threading.Tasks;
using GalaxyMerge.Archiving.Entities;

namespace GalaxyMerge.Archiving.Abstractions
{
    public interface IArchiveRepository : IDisposable
    {
        Archive GetArchive();
        Archive GetArchiveIncludeObjects();
        IArchiveObjectRepository Objects { get; }
        IQueueRepository Queue { get; }
        bool HasChanges();
        int Save();
        Task<int> SaveAsync();
    }
}