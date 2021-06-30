using System;
using System.ServiceModel;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Contracts;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;

namespace GalaxyMerge.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class GalaxyManager : IGalaxyService, IDisposable
    {
        private readonly IGalaxyRepositoryProvider _galaxyRepositoryProvider;
        private readonly IGalaxyDataRepositoryFactory _dataRepositoryFactory;
        private IGalaxyRepository _clientGrSession;
        private IGalaxyDataRepository _dataRepository;

        public GalaxyManager(IGalaxyRepositoryProvider galaxyRepositoryProvider, IGalaxyDataRepositoryFactory dataRepositoryFactory)
        {
            _galaxyRepositoryProvider = galaxyRepositoryProvider;
            _dataRepositoryFactory = dataRepositoryFactory;
        }

        public bool Connect(string galaxyName)
        {
            var galaxyConnectionString = DbStringBuilder.BuildGalaxy(Environment.MachineName, galaxyName);
            _dataRepository = _dataRepositoryFactory.Create(galaxyConnectionString);
            
            _clientGrSession = _galaxyRepositoryProvider.GetClientInstance(galaxyName);
            
            return _clientGrSession.Name == galaxyName && _clientGrSession.Connected;
        }

        public GalaxyObjectData GetObjectById(int objectId)
        {
            var tagName = _dataRepository.Objects.GetTagName(objectId);
            var galaxyObject = _clientGrSession.GetObject(tagName);
            return DataMapper.Map(galaxyObject);
        }

        public GalaxyObjectData GetObjectByName(string tagName)
        {
            var galaxyObject = _clientGrSession.GetObject(tagName);
            return DataMapper.Map(galaxyObject);
        }

        public GalaxySymbolData GetSymbolById(int objectId)
        {
            var tagName = _dataRepository.Objects.GetTagName(objectId);
            var galaxySymbol = _clientGrSession.GetSymbol(tagName);
            return DataMapper.Map(galaxySymbol);
        }

        public GalaxySymbolData GetSymbolByName(string tagName)
        {
            var galaxySymbol = _clientGrSession.GetSymbol(tagName);
            return DataMapper.Map(galaxySymbol);
        }

        public void Dispose()
        {
            _dataRepository?.Dispose();
        }
    }
}