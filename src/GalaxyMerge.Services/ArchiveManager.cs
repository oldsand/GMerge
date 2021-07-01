using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Contracts;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Services
{
    public class ArchiveManager : IArchiveService
    {
        private readonly IGalaxyRepositoryProvider _galaxyRepositoryProvider;
        private readonly IGalaxyDataRepositoryFactory _dataRepositoryFactory;
        private IGalaxyRepository _grSession;
        private IGalaxyDataRepository _dataRepository;

        public ArchiveManager(IGalaxyRepositoryProvider galaxyRepositoryProvider, IGalaxyDataRepositoryFactory dataRepositoryFactory)
        {
            _galaxyRepositoryProvider = galaxyRepositoryProvider;
            _dataRepositoryFactory = dataRepositoryFactory;
        }
        
        public bool Connect(string galaxyName)
        {
            var galaxyConnectionString = DbStringBuilder.BuildGalaxy(Environment.MachineName, galaxyName);
            _dataRepository = _dataRepositoryFactory.Create(galaxyConnectionString);
            
            _grSession = _galaxyRepositoryProvider.GetClientInstance(galaxyName);
            
            return _grSession.Name == galaxyName && _grSession.Connected;
        }
        
        public ArchiveObjectData GetArchiveObject(int objectId)
        {
            using var repo = new ArchiveRepository(_grSession.Name);
            var archiveObject = repo.GetObject(objectId);
            return DataMapper.Map(archiveObject);
        }

        public IEnumerable<ArchiveObjectData> GetArchiveObjects()
        {
            using var repo = new ArchiveRepository(_grSession.Name);
            var archiveObjects = repo.GetAllObjects();
            return archiveObjects.Select(DataMapper.Map);
        }

        public IEnumerable<ArchiveEntryData> GetArchiveEntries()
        {
            using var repo = new ArchiveRepository(_grSession.Name);
            var archiveEntries = repo.GetAllEntries();
            return archiveEntries.Select(DataMapper.Map);
        }

        public GalaxyObjectData GetGalaxyObject(int objectId)
        {
            using var archiveRepository = new ArchiveRepository(_grSession.Name);

            var archiveObject = archiveRepository.GetObjectIncludeEntries(objectId);
            if (archiveObject == null) return null;

            if (archiveObject.Template == Template.Symbol)
                throw new InvalidOperationException("Cannot convert symbol to object");

            var latest = archiveObject.Entries.OrderByDescending(e => e.ArchivedOn).First();
            return MaterializeObject(latest);
        }
        
        public GalaxySymbolData GetGalaxySymbol(int objectId)
        {
            using var archiveRepository = new ArchiveRepository(_grSession.Name);
            
            var archiveObject = archiveRepository.GetObjectIncludeEntries(objectId);
            if (archiveObject == null) return null;

            if (archiveObject.Template != Template.Symbol)
                throw new InvalidOperationException("Cannot convert symbol to object");

            var latest = archiveObject.Entries.OrderByDescending(e => e.ArchivedOn).FirstOrDefault();
            return MaterializeSymbol(latest);
        }

        public IEnumerable<EventSettingData> GetEventSettings()
        {
            using var repo = new ArchiveRepository(_grSession.Name);
            return repo.GetEventSettings().Select(DataMapper.Map);
        }

        public IEnumerable<InclusionSettingData> GetInclusionSettings()
        {
            using var repo = new ArchiveRepository(_grSession.Name);
            return repo.GetInclusionSettings().Select(DataMapper.Map);
        }

        public void AddObject(int objectId)
        {
            var archiver = new ArchiveProcessor(_grSession, _dataRepository);
            archiver.Archive(objectId);
        }

        public void RemoveObject(int objectId)
        {
            using var repo = new ArchiveRepository(_grSession.Name);
            repo.RemoveObject(objectId);
        }

        public void ArchiveObject(int objectId, bool force = false)
        {
            var archiver = new ArchiveProcessor(_grSession, _dataRepository);
            archiver.Archive(objectId);
        }

        public void UpdateEventSetting(IEnumerable<EventSettingData> eventSettings)
        {
            using var repo = new ArchiveRepository(_grSession.Name);
            repo.UpdateEventSettings(eventSettings.Select(x => new EventSetting(x.Operation, x.IsArchiveTrigger)));
        }

        public void UpdateInclusionSetting(IEnumerable<InclusionSettingData> inclusionSettings)
        {
            using var repo = new ArchiveRepository(_grSession.Name);
            repo.UpdateInclusionSettings(inclusionSettings.Select(x => new InclusionSetting(x.Template, x.InclusionOption, x.IncludeInstances)));
        }
        
        private static GalaxyObjectData MaterializeObject(ArchiveEntry latest)
        {
            var xml = XElement.Load(new MemoryStream(latest.CompressedData.Decompress()));
            var galaxyObject = new GalaxyObject().FromXml(xml);
            return DataMapper.Map(galaxyObject);
        }

        private static GalaxySymbolData MaterializeSymbol(ArchiveEntry latest)
        {
            var xml = XElement.Load(new MemoryStream(latest.CompressedData.Decompress()));
            var galaxyObject = new GalaxySymbol("").FromXml(xml); //todo figure out tagname here
            return DataMapper.Map(galaxyObject);
        }
    }
}