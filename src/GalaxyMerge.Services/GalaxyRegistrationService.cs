using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Services
{
    public class GalaxyRegistrationService
    {
        private readonly IGalaxyFinder _galaxyFinder;
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly string _serviceAccountName = "Admin"; //TODO Need to determine if I need a service account and how to get that.

        public GalaxyRegistrationService(IGalaxyFinder galaxyFinder, IGalaxyRegistry galaxyRegistry)
        {
            _galaxyFinder = galaxyFinder;
            _galaxyRegistry = galaxyRegistry;
        }
        
        public void Start()
        {
            var galaxies = _galaxyFinder.FindAll();
            foreach (var galaxy in galaxies)
                _galaxyRegistry.RegisterGalaxy(galaxy, _serviceAccountName);
        }

        public void Stop()
        {
            var galaxies = _galaxyRegistry.GetUserGalaxies(_serviceAccountName);
            foreach (var galaxy in galaxies)
                _galaxyRegistry.UnregisterGalaxy(galaxy.Name, _serviceAccountName);
        }
    }
}