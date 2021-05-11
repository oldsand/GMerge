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
        IEnumerable<ArchiveObject> GetAllObjects();
        ArchiveObject GetObjectIncludeEntries(int objectId);
        IEnumerable<ArchiveObject> FindAllByTagName(string tagName);
        bool HasEntries(int objectId);
        ArchiveEntry GetLatestEntry(int objectId);
        IEnumerable<ArchiveEntry> GetAllEntries();
        EventSetting GetEventSetting(int operationId);
        IEnumerable<EventSetting> GetEventSettings();
        InclusionSetting GetInclusionSetting(int templateId);
        IEnumerable<InclusionSetting> GetInclusionSettings();
        void AddObject(ArchiveObject archiveObject);
        void RemoveObject(ArchiveObject archiveObject);
        void RemoveObject(int objectId);
        void UpdateObject(ArchiveObject archiveObject);
        void UpdateGalaxyInfo(GalaxyInfo galaxyInfo);
        void UpdateEventSettings(IEnumerable<EventSetting> eventSettings);
        void UpdateInclusionSettings(IEnumerable<InclusionSetting> inclusionSettings);
        bool HasChanges();
        int Save();
        Task<int> SaveAsync();
    }
}