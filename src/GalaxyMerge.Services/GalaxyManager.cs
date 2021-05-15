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

        public GalaxyObjectData GetObjectById(int objectId)
        {
            using var objectRepository = new ObjectRepository(_clientGrSession.Name);
            var tagName = objectRepository.GetTagName(objectId);
            var galaxyObject = _clientGrSession.GetObject(tagName);
            return Mapper.Map(galaxyObject);
        }

        public GalaxyObjectData GetObjectByName(string tagName)
        {
            var galaxyObject = _clientGrSession.GetObject(tagName);
            return Mapper.Map(galaxyObject);
        }

        public GalaxySymbolData GetSymbolById(int objectId)
        {
            using var objectRepository = new ObjectRepository(_clientGrSession.Name);
            var tagName = objectRepository.GetTagName(objectId);
            var galaxySymbol = _clientGrSession.GetSymbol(tagName);
            return Mapper.Map(galaxySymbol);
        }

        public GalaxySymbolData GetSymbolByName(string tagName)
        {
            var galaxySymbol = _clientGrSession.GetSymbol(tagName);
            return Mapper.Map(galaxySymbol);
        }
    }
}