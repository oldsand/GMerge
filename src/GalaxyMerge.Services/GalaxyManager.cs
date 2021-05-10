using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Xml.Linq;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Contracts;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class GalaxyManager : IGalaxyService
    {
        private readonly IGalaxyRepositoryProvider _galaxyRepositoryProvider;
        private IGalaxyRepository _clientGrSession;

        public GalaxyManager(IGalaxyRepositoryProvider galaxyRepositoryProvider)
        {
            _galaxyRepositoryProvider = galaxyRepositoryProvider;
        }

        public bool Connect(string galaxyName)
        {
            _clientGrSession = _galaxyRepositoryProvider.GetClientInstance(galaxyName);
            return _clientGrSession.Name == galaxyName && _clientGrSession.Connected;
        }

        public GalaxyObject GetObject(int objectId)
        {
            //todo should probably split this out to archive manager to allow client cde to determine where to get object from.
            using var archiveRepository = new ArchiveRepository(_clientGrSession.Name);
            var exists = archiveRepository.ObjectExists(objectId);
            if (exists)
            {
                var latest = archiveRepository.GetLatestEntry(objectId);
                var xml = XElement.Load(new MemoryStream(latest.CompressedData.Decompress()));
                var galaxyObject = new GalaxyObject().FromXml(xml);
                return (GalaxyObject) galaxyObject;
            }
            
            using var objectRepository = new ObjectRepository(_clientGrSession.Name);
            var tagName = objectRepository.GetTagName(objectId);
            return (GalaxyObject) _clientGrSession.GetObject(tagName);
        }

        public GalaxyObject GetObjects(string tagName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GalaxyObject> GetObjects(IEnumerable<int> objectIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GalaxyObject> GetObjects(IEnumerable<string> tagNames)
        {
            throw new NotImplementedException();
        }
    }
}