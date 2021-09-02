using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;
using GServer.Archestra.Abstractions;
using GCommon.Archiving.Abstractions;
using GCommon.Contracts;
using GCommon.Core.Extensions;
using GCommon.Core.Utilities;
using GCommon.Data.Abstractions;
using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using GServer.Services.Abstractions;

namespace GServer.Services
{
    public class ArchiveManager : IArchiveService
    {
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly IGalaxyDataProviderFactory _galaxyDataProviderFactory;
        private readonly IArchiveRepositoryFactory _archiveRepositoryFactory;
        private IGalaxyRepository _galaxyRepository;

        public ArchiveManager(IGalaxyRegistry galaxyRegistry,
            IGalaxyDataProviderFactory galaxyDataProviderFactory,
            IArchiveRepositoryFactory archiveRepositoryFactory)
        {
            _galaxyRegistry = galaxyRegistry;
            _galaxyDataProviderFactory = galaxyDataProviderFactory;
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
            var archiveObject = repo.GetObject(objectId);
            return DataMapper.Map(archiveObject);
        }

        public IEnumerable<ArchiveObjectData> GetArchiveObjects()
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archiveObjects = repo.GetObjects();
            return archiveObjects.Select(DataMapper.Map);
        }

        public IEnumerable<ArchiveEntryData> GetArchiveEntries()
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archiveEntries = repo.GetObjects().SelectMany(o => o.Entries);
            return archiveEntries.Select(DataMapper.Map);
        }

        public GalaxyObjectData GetGalaxyObject(int objectId)
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archiveObject = repo.GetObject(objectId);
            if (archiveObject == null) return null;

            if (archiveObject.Template == Template.Symbol)
                throw new InvalidOperationException("Cannot convert symbol to object");

            var latest = archiveObject.Entries.OrderByDescending(e => e.ArchivedOn).First();
            return MaterializeObject(latest);
        }

        public GalaxySymbolData GetGalaxySymbol(int objectId)
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var archiveObject = repo.GetObject(objectId);
            if (archiveObject == null) return null;

            if (archiveObject.Template != Template.Symbol)
                throw new InvalidOperationException("Cannot convert symbol to object");

            var latest = archiveObject.Entries.OrderByDescending(e => e.ArchivedOn).FirstOrDefault();
            return MaterializeSymbol(latest);
        }

        public IEnumerable<EventSettingData> GetEventSettings()
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var config = repo.GetConfig();
            return config.EventSettings.Select(DataMapper.Map);
        }

        public IEnumerable<InclusionSettingData> GetInclusionSettings()
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            var config = repo.GetConfig();
            return config.InclusionSettings.Select(DataMapper.Map);
        }

        public void ArchiveObject(int objectId)
        {
            using var dataProvider = _galaxyDataProviderFactory
                .Create(DbStringBuilder.GalaxyString(_galaxyRepository.Name));
            using var archiveRepository = _archiveRepositoryFactory
                .Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            using var archiver = new GalaxyArchiver(_galaxyRepository, archiveRepository);

            var gObject = dataProvider.Objects.Find(objectId);
            var archiveObject = 
                new ArchiveObject(gObject.ObjectId, gObject.TagName, gObject.ConfigVersion, gObject.Template);

            archiver.Archive(archiveObject);
        }

        public void RemoveObject(int objectId)
        {
            using var repo = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyRepository.Name));
            repo.DeleteObject(objectId);
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
            var element = XElement.Load(new MemoryStream(latest.CompressedData.Decompress()));
            var archestraObject = ArchestraObject.Materialize(element);
            return DataMapper.Map(archestraObject);
        }

        //todo figure out tag name here
        private static GalaxySymbolData MaterializeSymbol(ArchiveEntry latest)
        {
            var xml = XElement.Load(new MemoryStream(latest.CompressedData.Decompress()));
            var galaxyObject = new ArchestraGraphic("").Materialize(xml);
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