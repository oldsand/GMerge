using System;
using System.ServiceModel;
using GServer.Archestra.Abstractions;
using GCommon.Contracts;
using GCommon.Data.Abstractions;
using GCommon.Utilities;
using GServer.Services.Abstractions;

namespace GServer.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class GalaxyManager : IGalaxyService, IDisposable
    {
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly IGalaxyDataProviderFactory _galaxyDataProviderFactory;
        private IGalaxyRepository _galaxyRepository;
        private IGalaxyDataProvider _galaxyDataProvider;

        public GalaxyManager(IGalaxyRegistry galaxyRegistry, IGalaxyDataProviderFactory galaxyDataProviderFactory)
        {
            _galaxyRegistry = galaxyRegistry ?? throw new ArgumentNullException(nameof(galaxyRegistry));
            _galaxyDataProviderFactory =
                galaxyDataProviderFactory ?? throw new ArgumentNullException(nameof(galaxyDataProviderFactory));
        }

        public bool Connect(string galaxyName)
        {
            var connectionString = DbStringBuilder.GalaxyString(Environment.MachineName, galaxyName);
            _galaxyDataProvider = _galaxyDataProviderFactory.Create(connectionString);

            _galaxyRepository = GetRegisteredGalaxy(galaxyName);

            return _galaxyRepository.Name == galaxyName && _galaxyRepository.Connected;
        }

        public GalaxyObjectData GetObjectById(int objectId)
        {
            ValidateInitialization();

            var tagName = _galaxyDataProvider.Objects.GetTagName(objectId);
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
            
            var tagName = _galaxyDataProvider.Objects.GetTagName(objectId);
            var galaxySymbol = _galaxyRepository.GetGraphic(tagName);
            return DataMapper.Map(galaxySymbol);
        }

        public GalaxySymbolData GetSymbolByName(string tagName)
        {
            ValidateInitialization();
            
            var galaxySymbol = _galaxyRepository.GetGraphic(tagName);
            return DataMapper.Map(galaxySymbol);
        }

        public void Dispose()
        {
            _galaxyDataProvider?.Dispose();
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

            if (_galaxyDataProvider == null)
                throw new InvalidOperationException(
                    "Data Repository not initialized. Can not perform call on uninitialized service. Call Connect prior to using service.");
        }

    }
}