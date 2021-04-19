using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Archestra
{
    public class GalaxyRegistry : IGalaxyRegistry
    {
        private readonly IGalaxyFactory _galaxyFactory;
        private readonly IGalaxyFinder _galaxyFinder;
        private readonly List<IGalaxyRepository> _galaxies = new List<IGalaxyRepository>();

        public GalaxyRegistry()
        {
            _galaxyFactory = new GalaxyFactory();
            _galaxyFinder = new GalaxyFinder();
        }

        internal GalaxyRegistry(IGalaxyFactory galaxyFactory, IGalaxyFinder galaxyFinder)
        {
            _galaxyFactory = galaxyFactory;
            _galaxyFinder = galaxyFinder;
        }

        public bool IsGalaxyRegistered(string galaxyName, string userName)
        {
            return _galaxies.SingleOrDefault(g => g.Name == galaxyName && g.LoggedInUser == userName) != null;
        }

        public IGalaxyRepository GetGalaxy(string galaxyName, string userName)
        {
            return _galaxies.SingleOrDefault(g => g.Name == galaxyName && g.LoggedInUser == userName);
        }

        public IEnumerable<IGalaxyRepository> GetUserGalaxies(string userName)
        {
            return _galaxies.Where(g => g.LoggedInUser == userName);
        }

        public IEnumerable<IGalaxyRepository> GetAllGalaxies()
        {
            return _galaxies;
        }

        public void RegisterGalaxy(string galaxyName, string userName)
        {
            if (IsGalaxyRegistered(galaxyName, userName)) return;
            var galaxy = _galaxyFactory.Create(galaxyName);
            galaxy.Login(userName);
            _galaxies.Add(galaxy);
        }
        
        public async Task RegisterGalaxyAsync(string galaxyName, string userName, CancellationToken token)
        {
            if (IsGalaxyRegistered(galaxyName, userName)) return;
            var galaxy = await _galaxyFactory.CreateAsync(galaxyName, token);
            await galaxy.LoginAsync(userName, token);
            _galaxies.Add(galaxy);
        }
        
        public async Task RegisterGalaxiesAsync(string userName, CancellationToken token)
        {
            var galaxies = (await _galaxyFinder.FindAllAsync(token)).ToList();
            var creationTasks = galaxies.Select(g => _galaxyFactory.CreateAsync(g, token)).ToList();

            UnregisterGalaxies(galaxies, userName);
            
            while (creationTasks.Any())
            {
                var connection = await Task.WhenAny(creationTasks);
                creationTasks.Remove(connection);
                var galaxy = connection.Result;
                await galaxy.LoginAsync(userName, token);
                _galaxies.Add(galaxy);
            }
        }

        public void UnregisterGalaxy(string galaxyName, string userName)
        {
            if (!IsGalaxyRegistered(galaxyName, userName)) return;
            var galaxy = GetGalaxy(galaxyName, userName);
            _galaxies.Remove(galaxy);
            galaxy.Logout();
        }

        public void UnregisterGalaxies(IEnumerable<string> galaxies, string userName)
        {
            foreach (var galaxy in galaxies)
                UnregisterGalaxy(galaxy, userName);
        }
    }
}