using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
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
    public class ArchiveManager : IArchiveService, IDisposable
    {
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly IGalaxyDataRepositoryFactory _dataRepositoryFactory;
        private ArchiveProcessor _archiver;
        private ArchiveRepository _archiveRepository;

        public ArchiveManager(IGalaxyRegistry galaxyRegistry, IGalaxyDataRepositoryFactory dataRepositoryFactory)
        {
            _galaxyRegistry = galaxyRegistry;
            _dataRepositoryFactory = dataRepositoryFactory;
        }
        
        public bool Connect(string galaxyName)
        {
            var connectionString = DbStringBuilder.BuildGalaxy(Environment.MachineName, galaxyName);
            var dataRepository = _dataRepositoryFactory.Create(connectionString);
            var galaxyRepository = GetRegisteredGalaxy(galaxyName);
            
            _archiver = new ArchiveProcessor(galaxyRepository, dataRepository);
            
            //todo create and replace with factory interface injected into constructor
            _archiveRepository = new ArchiveRepository(galaxyName);

            return galaxyRepository.Name == galaxyName && galaxyRepository.Connected;
        }
        
        public ArchiveObjectData GetArchiveObject(int objectId)
        {
            ValidateInitialization();

            var archiveObject = _archiveRepository.GetObject(objectId);
            return DataMapper.Map(archiveObject);
        }

        public IEnumerable<ArchiveObjectData> GetArchiveObjects()
        {
            ValidateInitialization();
            
            var archiveObjects = _archiveRepository.GetAllObjects();
            return archiveObjects.Select(DataMapper.Map);
        }

        public IEnumerable<ArchiveEntryData> GetArchiveEntries()
        {
            
            var archiveEntries = _archiveRepository.GetAllEntries();
            return archiveEntries.Select(DataMapper.Map);
        }

        public GalaxyObjectData GetGalaxyObject(int objectId)
        {
            

            var archiveObject = _archiveRepository.GetObjectIncludeEntries(objectId);
            if (archiveObject == null) return null;

            if (archiveObject.Template == Template.Symbol)
                throw new InvalidOperationException("Cannot convert symbol to object");

            var latest = archiveObject.Entries.OrderByDescending(e => e.ArchivedOn).First();
            return MaterializeObject(latest);
        }
        
        public GalaxySymbolData GetGalaxySymbol(int objectId)
        {
            
            
            var archiveObject = _archiveRepository.GetObjectIncludeEntries(objectId);
            if (archiveObject == null) return null;

            if (archiveObject.Template != Template.Symbol)
                throw new InvalidOperationException("Cannot convert symbol to object");

            var latest = archiveObject.Entries.OrderByDescending(e => e.ArchivedOn).FirstOrDefault();
            return MaterializeSymbol(latest);
        }

        public IEnumerable<EventSettingData> GetEventSettings()
        {
            return _archiveRepository.GetEventSettings().Select(DataMapper.Map);
        }

        public IEnumerable<InclusionSettingData> GetInclusionSettings()
        {
            return _archiveRepository.GetInclusionSettings().Select(DataMapper.Map);
        }

        public void AddObject(int objectId)
        {
            _archiver.Archive(objectId);
        }

        public void RemoveObject(int objectId)
        {
            _archiveRepository.RemoveObject(objectId);
        }

        public void ArchiveObject(int objectId, bool force = false)
        {
            ValidateInitialization();
            
            _archiver.Archive(objectId);
        }

        public void UpdateEventSetting(IEnumerable<EventSettingData> eventSettings)
        {
            _archiveRepository.UpdateEventSettings(eventSettings.Select(x => 
                new EventSetting(x.Operation, x.IsArchiveTrigger)));
        }

        public void UpdateInclusionSetting(IEnumerable<InclusionSettingData> inclusionSettings)
        {
            _archiveRepository.UpdateInclusionSettings(inclusionSettings.Select(x => 
                new InclusionSetting(x.Template, x.InclusionOption, x.IncludeInstances)));
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
        
        public void Dispose()
        {
            _archiveRepository?.Dispose();
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
        
        private void ValidateInitialization()
        {
            if (_archiver == null)
                throw new InvalidOperationException(
                    "Archive Processor not initialized. Can not perform call on uninitialized service. Call Connect prior to using service.");

            if (_archiveRepository == null)
                throw new InvalidOperationException(
                    "Archive Repository not initialized. Can not perform call on uninitialized service. Call Connect prior to using service.");
        }

    }
}