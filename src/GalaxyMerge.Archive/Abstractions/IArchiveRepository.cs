using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyMerge.Archive.Entities;

namespace GalaxyMerge.Archive.Abstractions
{
    public interface IArchiveRepository : IDisposable
    {
        ArchiveInfo GetInfo();
        bool ObjectExists(int objectId);
        ArchiveObject GetObject(int objectId);
        ArchiveObject GetObjectIncludeEntries(int objectId);
        IEnumerable<ArchiveObject> FindObjectsByTagName(string tagName);
        bool HasEntries(int objectId);
        IEnumerable<ArchiveEntry> FindEntriesByObjectId(int objectId);
        ArchiveEntry GetLatestEntry(int objectId);
        bool HasEvent(string eventName);
        ArchiveEvent GetEvent(int eventId);
        IEnumerable<ArchiveEvent> GetEvents();
        bool HasExclusion(string exclusionName);
        ArchiveTemplate GetExclusion(int exclusionId);
        IEnumerable<ArchiveTemplate> GetExclusions();
        void UpdateInfo(ArchiveInfo archiveInfo);
        void AddObject(ArchiveObject archiveObject);
        void RemoveObject(ArchiveObject archiveObject);
        void UpdateObject(ArchiveObject archiveObject);
        void AddEntry(ArchiveEntry archiveEntry);
        void AddEvent(ArchiveEvent archiveEvent);
        void RemoveEvent(ArchiveEvent archiveEvent);
        void AddExclusion(ArchiveTemplate archiveTemplate);
        void RemoveExclusion(ArchiveTemplate archiveTemplate);
        bool HasChanges();
        int Save();
        Task<int> SaveAsync();
    }
}