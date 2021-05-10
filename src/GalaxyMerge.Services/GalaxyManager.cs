using System;
using System.Collections.Generic;
using System.ServiceModel;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Contracts;
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