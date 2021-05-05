using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyMerge.Archive.Entities;

namespace GalaxyMerge.Archive.Abstractions
{
    public interface IArchiveRepository : IDisposable
    {
        GalaxyInfo GetGalaxyInfo();
        bool ObjectExists(int objectId);
        ArchiveObject GetObject(int objectId);
        ArchiveObject GetObjectIncludeEntries(int objectId);
        IEnumerable<ArchiveObject> FindObjectsByTagName(string tagName);
        bool HasEntries(int objectId);
        IEnumerable<ArchiveEntry> FindEntriesByObjectId(int objectId);
        ArchiveEntry GetLatestEntry(int objectId);
        EventSetting GetEventSetting(int eventId);
        IEnumerable<EventSetting> GetEventSettings();
        InclusionSetting GetInclusionSetting(int exclusionId);
        IEnumerable<InclusionSetting> GetInclusionSettings();
        void UpdateGalaxyInfo(GalaxyInfo galaxyInfo);
        void AddObject(ArchiveObject archiveObject);
        void RemoveObject(ArchiveObject archiveObject);
        void UpdateObject(ArchiveObject archiveObject);
        void AddEntry(ArchiveEntry archiveEntry);
        void AddEvent(EventSetting eventSetting);
        void RemoveEvent(EventSetting eventSetting);
        void UpdateEvent(EventSetting eventSetting);
        void AddInclusion(InclusionSetting inclusionSetting);
        void RemoveInclusion(InclusionSetting inclusionSetting);
        void UpdateInclusion(InclusionSetting inclusionSetting);
        bool HasChanges();
        int Save();
        Task<int> SaveAsync();
    }
}