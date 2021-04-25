using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyMerge.Archive.Entities;

namespace GalaxyMerge.Archive.Abstractions
{
    public interface IArchiveRepository : IDisposable
    {
        ArchiveInfo GetInfo();
        ArchiveEntry GetEntry(Guid id);
        IEnumerable<ArchiveEntry> FindByObjectId(int objectId);
        IEnumerable<ArchiveEntry> FindByTagName(string tagName);
        ArchiveEntry GetLatest(int objectId);
        ArchiveEntry GetLatest(string tagName);
        bool HasEntries(string tagName);
        void AddInfo(ArchiveInfo archiveInfo);
        void RemoveInfo(ArchiveInfo archiveInfo);
        void UpdateInfo(ArchiveInfo archiveInfo);
        void AddEntry(ArchiveEntry archiveEntry);
        void RemoveEntry(ArchiveEntry archiveEntry);
        void UpdateEntry(ArchiveEntry archiveEntry);
        bool HasChanges();
        int Save();
        Task<int> SaveAsync();
    }
}