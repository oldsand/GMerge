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
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly IGalaxyDataRepositoryFactory _dataRepositoryFactory;
        private IGalaxyRepository _galaxyRepository;
        private IGalaxyDataRepository _dataRepository;

        public GalaxyManager(IGalaxyRegistry galaxyRegistry, IGalaxyDataRepositoryFactory dataRepositoryFactory)
        {
            _galaxyRegistry = galaxyRegistry ?? throw new ArgumentNullException(nameof(galaxyRegistry));
            _dataRepositoryFactory =
                dataRepositoryFactory ?? throw new ArgumentNullException(nameof(dataRepositoryFactory));
        }

        public bool Connect(string galaxyName)
        {
            var connectionString = DbStringBuilder.BuildGalaxy(Environment.MachineName, galaxyName);
            _dataRepository = _dataRepositoryFactory.Create(connectionString);

            _galaxyRepository = GetRegisteredGalaxy(galaxyName);

            return _galaxyRepository.Name == galaxyName && _galaxyRepository.Connected;
        }

        public GalaxyObjectData GetObjectById(int objectId)
        {
            ValidateInitialization();

            var tagName = _dataRepository.Objects.GetTagName(objectId);
            var galaxyObject = _galaxyRepository.GetObject(tagName);
            return DataMapper.Map(galaxyObject);
        }
        
        public GalaxyObjectData GetObjectByName(string tagName)
        {
            ValidateInitialization();
            
            var galaxyObject = _galaxyRepository.GetObject(tagName);
            return DataMapper.Map(galaxyObject);
        }

        public GalaxySymbolData GetSymbolById(int objectId)
        {
            ValidateInitialization();
            
            var tagName = _dataRepository.Objects.GetTagName(objectId);
            var galaxySymbol = _galaxyRepository.GetSymbol(tagName);
            return DataMapper.Map(galaxySymbol);
        }

        public GalaxySymbolData GetSymbolByName(string tagName)
        {
            ValidateInitialization();
            
            var galaxySymbol = _galaxyRepository.GetSymbol(tagName);
            return DataMapper.Map(galaxySymbol);
        }

        public void Dispose()
        {
            _dataRepository?.Dispose();
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
            if (_galaxyRepository == null)
                throw new InvalidOperationException(
                    "Galaxy Repository not initialized. Can not perform call on uninitialized service. Call Connect prior to using service.");

            if (_dataRepository == null)
                throw new InvalidOperationException(
                    "Data Repository not initialized. Can not perform call on uninitialized service. Call Connect prior to using service.");
        }

    }
}