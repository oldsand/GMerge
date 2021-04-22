using System.Security.Principal;
using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Services
{
    public class GalaxyRegistrant
    {
        private readonly IGalaxyFinder _galaxyFinder;
        private readonly IGalaxyRegistry _galaxyRegistry;

        public GalaxyRegistrant(IGalaxyFinder galaxyFinder, IGalaxyRegistry galaxyRegistry)
        {
            _galaxyFinder = galaxyFinder;
            _galaxyRegistry = galaxyRegistry;
        }
        
        public void RunRegistration()
        {
            var user = WindowsIdentity.GetCurrent();
            var galaxies = _galaxyFinder.FindAll();
            
            foreach (var galaxy in galaxies)
                _galaxyRegistry.RegisterGalaxy(galaxy, user.Name);
        }

        public void Unregister()
        {
            var user = WindowsIdentity.GetCurrent();
            var galaxies = _galaxyRegistry.GetUserGalaxies(user.Name);
            
            foreach (var galaxy in galaxies)
                _galaxyRegistry.UnregisterGalaxy(galaxy.Name, user.Name);
        }
    }
}