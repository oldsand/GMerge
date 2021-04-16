using System.ServiceModel;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Contracts;
using GalaxyMerge.Contracts.Services;

namespace GalaxyMerge.Services
{
    public class GalaxyRegistrationManager : IGalaxyRegistrationService
    {
        private readonly IGalaxyRegistry _galaxyRegistry;

        public GalaxyRegistrationManager(IGalaxyRegistry galaxyRegistry)
        {
            _galaxyRegistry = galaxyRegistry;
        }
        
        public void RegisterGalaxy(string galaxyName)
        {
            //TODO: Get service calling user and authorize their access to the machine and Archestra Galaxy
            var clientUserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            _galaxyRegistry.RegisterGalaxy(galaxyName, clientUserName);
        }

        public void UnregisterGalaxy(string galaxyName)
        {
            var clientUserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            _galaxyRegistry.UnregisterGalaxy(galaxyName, clientUserName);
        }
    }
}