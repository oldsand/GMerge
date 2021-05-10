using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Principal;
using System.ServiceModel;
using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Services
{
    public class GalaxyRepositoryProvider : IGalaxyRepositoryProvider
    {
        private readonly IGalaxyRegistry _galaxyRegistry;

        public GalaxyRepositoryProvider(IGalaxyRegistry galaxyRegistry)
        {
            _galaxyRegistry = galaxyRegistry;
        }

        public IEnumerable<IGalaxyRepository> GetAllServiceInstances()
        {
            var serviceUser = WindowsIdentity.GetCurrent();
            return _galaxyRegistry.GetByUser(serviceUser.Name);
        }

        public IGalaxyRepository GetServiceInstance(string galaxyName)
        {
            return GetServiceGr(galaxyName);
        }
        
        public IGalaxyRepository GetClientInstance(string galaxyName)
        {
            return GetClientGr(galaxyName);
        }

        private IGalaxyRepository GetServiceGr(string galaxyName)
        {
            var serviceUser = WindowsIdentity.GetCurrent();
            
            var galaxyRepository = _galaxyRegistry.GetGalaxy(galaxyName, serviceUser.Name);
            if (galaxyRepository == null)
                throw new InvalidOperationException(
                    $"Cannot find registered galaxy with name '{galaxyName}' for current service user '{serviceUser.Name}'");

            return galaxyRepository;
        }
        
        private IGalaxyRepository GetClientGr(string galaxyName)
        {
            var clientUserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            
            if (!_galaxyRegistry.IsRegistered(galaxyName, clientUserName))
                RegisterGalaxyToClient(galaxyName, clientUserName);
            
            var galaxyRepository = _galaxyRegistry.GetGalaxy(galaxyName, clientUserName);
            if (galaxyRepository == null)
                throw new InvalidOperationException(
                    $"Cannot find registered galaxy with name '{galaxyName}' for current client user '{clientUserName}'");
            
            galaxyRepository.SynchronizeClient();
            return galaxyRepository;
        }

        private void RegisterGalaxyToClient(string galaxyName, string userName)
        {
            var galaxyRepository = GetServiceGr(galaxyName);
            if (!galaxyRepository.UserIsAuthorized(userName))
                throw new SecurityException($"User '{userName}' does not have access to galaxy '{galaxyName}'");
            
            _galaxyRegistry.Register(galaxyName, userName);
        }

        
    }
}