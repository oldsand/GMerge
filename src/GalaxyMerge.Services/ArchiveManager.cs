using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Contracts;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Primitives;
using GalaxyMerge.Services.Abstractions;

namespace GalaxyMerge.Services
{
    public class ArchiveManager : IArchiveService
    {
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly IDataRepositoryFactory _dataRepositoryFactory;
        private readonly IArchiveRepositoryFactory _archiveRepositoryFactory;
        private IGalaxyRepository _galaxyRepository;

        public ArchiveManager(IGalaxyRegistry galaxyRegistry,
            IDataRepositoryFactory dataRepositoryFactory,
            IArchiveRepositoryFactory archiveRepositoryFactory,
            IArchiveProcessorFactory processorFactory)
        {
            _galaxyRegistry = galaxyRegistry;
            _dataRepositoryFactory = dataRepositoryFactory;
            _archiveRepositoryFactory = archiveRepositoryFactory;
        }
        
        public bool Connect(string galaxyName)
        {
            _galaxyRepository = GetRegisteredGalaxy(galaxyName);
            return _galaxyRepository.Name == galaxyName && _galaxyRepository.Connected;
        }
        
        public ArchiveObjectData GetArchiveObject(int objectId)
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archiveObject = repo.Objects.FindInclude(objectId);
            return DataMapper.Map(archiveObject);
        }

        public IEnumerable<ArchiveObjectData> GetArchiveObjects()
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archiveObjects = repo.Objects.GetAll();
            return archiveObjects.Select(DataMapper.Map);
        }

        public IEnumerable<ArchiveEntryData> GetArchiveEntries()
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archiveEntries = repo.Objects.GetAll().SelectMany(o => o.Entries);
            return archiveEntries.Select(DataMapper.Map);
        }

        public GalaxyObjectData GetGalaxyObject(int objectId)
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archiveObject = repo.Objects.FindInclude(objectId);
            if (archiveObject == null) return null;

            if (archiveObject.Template == Template.Symbol)
                throw new InvalidOperationException("Cannot convert symbol to object");

            var latest = archiveObject.Entries.OrderByDescending(e => e.ArchivedOn).First();
            return MaterializeObject(latest);
        }
        
        public GalaxySymbolData GetGalaxySymbol(int objectId)
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archiveObject = repo.Objects.FindInclude(objectId);
            if (archiveObject == null) return null;

            if (archiveObject.Template != Template.Symbol)
                throw new InvalidOperationException("Cannot convert symbol to object");

            var latest = archiveObject.Entries.OrderByDescending(e => e.ArchivedOn).FirstOrDefault();
            return MaterializeSymbol(latest);
        }

        public IEnumerable<EventSettingData> GetEventSettings()
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archive = repo.Get();
            return archive.EventSettings.Select(DataMapper.Map);
        }

        public IEnumerable<InclusionSettingData> GetInclusionSettings()
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archive = repo.Get();
            return archive.InclusionSettings.Select(DataMapper.Map);
        }

        public void AddObject(int objectId)
        {
            using var dataRepo = _dataRepositoryFactory.Create(DbStringBuilder.GalaxyString(_galaxyRepository.Name));
            using var archiveRepo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            using var archiver = new Archiver(_galaxyRepository, dataRepo, archiveRepo);
            archiver.Archive(objectId);
        }

        public void RemoveObject(int objectId)
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            repo.Objects.Remove(objectId);
        }

        public void ArchiveObject(int objectId, bool force = false)
        {
            using var dataRepo = _dataRepositoryFactory.Create(DbStringBuilder.GalaxyString(_galaxyRepository.Name));
            using var archiveRepo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            using var archiver = new Archiver(_galaxyRepository, dataRepo, archiveRepo);
            archiver.Archive(objectId, force);
        }

        public void UpdateEventSetting(IEnumerable<EventSettingData> eventSettings)
        {
            /*_archiveRepository.UpdateEventSettings(eventSettings.Select(x => 
                new EventSetting(x.Operation, x.IsArchiveTrigger)));*/
        }

        public void UpdateInclusionSetting(IEnumerable<InclusionSettingData> inclusionSettings)
        {
            /*_archiveRepository.UpdateInclusionSettings(inclusionSettings.Select(x => 
                new InclusionSetting(x.Template, x.InclusionOption, x.IncludeInstances)));*/
        }
        
        private static GalaxyObjectData MaterializeObject(ArchiveEntry latest)
        {
            var xml = XElement.Load(new MemoryStream(latest.CompressedData.Decompress()));
            var galaxyObject = new GalaxyObject().FromXml(xml);
            return DataMapper.Map(galaxyObject);
        }

        //todo figure out tag name here
        private static GalaxySymbolData MaterializeSymbol(ArchiveEntry latest)
        {
            var xml = XElement.Load(new MemoryStream(latest.CompressedData.Decompress()));
            var galaxyObject = new GalaxySymbol("").FromXml(xml); 
            return DataMapper.Map(galaxyObject);
        }

        private IGalaxyRepository GetRegisteredGalaxy(string galaxyName)
        {
            var userName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            if (userName == null)
                throw new InvalidOperationException("Could not get user name from current primary identity");

            _galaxyRegistry.Register(galaxyName, userName);

            var galaxyRepository = _galaxyRegistry.GetGalaxy(galaxyName, userName);
            if (galaxyRepository == null)
                throw new InvalidOperationException(
                    $"Cannot find registered galaxy with name '{galaxyName}' for current client user '{userName}'");

            return galaxyRepository;
        }
    }
}