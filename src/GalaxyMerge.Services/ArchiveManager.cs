using System.Collections.Generic;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Contracts;

namespace GalaxyMerge.Services
{
    public class ArchiveManager : IArchiveService
    {
        private readonly IGalaxyRepositoryProvider _galaxyRepositoryProvider;
        private IGalaxyRepository _clientGrSession;

        public ArchiveManager(IGalaxyRepositoryProvider galaxyRepositoryProvider)
        {
            _galaxyRepositoryProvider = galaxyRepositoryProvider;
        }
        
        public bool Connect(string galaxyName)
        {
            _clientGrSession = _galaxyRepositoryProvider.GetClientInstance(galaxyName);
            return _clientGrSession.Name == galaxyName && _clientGrSession.Connected;
        }
        
        public ArchiveObject GetObject(int objectId)
        {
            using var repo = new ArchiveRepository(_clientGrSession.Name);
            return repo.GetObject(objectId);
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
            using var repo = new ArchiveRepository(_clientGrSession.Name);
            return repo.GetEventSettings();
        }

        public IEnumerable<InclusionSetting> GetInclusionSettings()
        {
            using var repo = new ArchiveRepository(_clientGrSession.Name);
            return repo.GetInclusionSettings();
        }

        public void AddObject(int objectId)
        {
            var archiver = new ArchiveProcessor(_clientGrSession);
            archiver.Archive(objectId);
        }

        public void RemoveObject(int objectId)
        {
            using var repo = new ArchiveRepository(_clientGrSession.Name);
            repo.RemoveObject(objectId);
        }

        public void ArchiveObject(int objectId, bool force = false)
        {
            var archiver = new ArchiveProcessor(_clientGrSession);
            archiver.Archive(objectId);
        }

        public void UpdateEventSetting(IEnumerable<EventSetting> eventSettings)
        {
            using var repo = new ArchiveRepository(_clientGrSession.Name);
            repo.UpdateEventSettings(eventSettings);
        }

        public void UpdateInclusionSetting(IEnumerable<InclusionSetting> inclusionSettings)
        {
            using var repo = new ArchiveRepository(_clientGrSession.Name);
            repo.UpdateInclusionSettings(inclusionSettings);
        }
    }
}