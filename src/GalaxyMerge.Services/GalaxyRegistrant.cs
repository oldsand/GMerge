using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Services
{
    public class GalaxyRegistrant
    {
        private readonly IGalaxyFinder _galaxyFinder;
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly string _serviceAccountName = "admin"; //TODO Need to determine if I need a service account and how to get that.

        public GalaxyRegistrant(IGalaxyFinder galaxyFinder, IGalaxyRegistry galaxyRegistry)
        {
            _galaxyFinder = galaxyFinder;
            _galaxyRegistry = galaxyRegistry;
        }
        
        public void RunRegistration()
        {
            var galaxies = _galaxyFinder.FindAll();
            foreach (var galaxy in galaxies)
                _galaxyRegistry.RegisterGalaxy(galaxy, _serviceAccountName);
        }

        public void Unregister()
        {
            var galaxies = _galaxyRegistry.GetUserGalaxies(_serviceAccountName);
            foreach (var galaxy in galaxies)
                _galaxyRegistry.UnregisterGalaxy(galaxy.Name, _serviceAccountName);
        }
    }
}