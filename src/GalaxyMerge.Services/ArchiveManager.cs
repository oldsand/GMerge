using System.Collections.Generic;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Contracts;

namespace GalaxyMerge.Services
{
    public class ArchiveManager : IArchiveService
    {
        public ArchiveObject GetObject(int objectId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ArchiveObject> GetObjects(string tagName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ArchiveEntry> GetEntries(int objectId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<EventSetting> GetEventSettings()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<InclusionSetting> GetInclusionSettings()
        {
            throw new System.NotImplementedException();
        }

        public void AddObject(int objectId)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveObject(int objectId)
        {
            throw new System.NotImplementedException();
        }

        public void ArchiveObject(int objectId)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateEventSetting(Operation operation, bool isArchiveEvent)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateInclusionSetting(Template template, InclusionOption option, bool includeInstances)
        {
            throw new System.NotImplementedException();
        }
    }
}