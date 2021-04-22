using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaxyMerge.Archestra;
using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Services
{
    public class GalaxyRegistry : IGalaxyRegistry
    {
        private readonly IGalaxyRepositoryFactory _repositoryFactory;
        private readonly IGalaxyFinder _galaxyFinder;
        private readonly List<IGalaxyRepository> _galaxies = new List<IGalaxyRepository>();

        public GalaxyRegistry()
        {
            _repositoryFactory = new GalaxyRepositoryFactory();
            _galaxyFinder = new GalaxyFinder();
        }

        internal GalaxyRegistry(IGalaxyRepositoryFactory repositoryFactory, IGalaxyFinder galaxyFinder)
        {
            _repositoryFactory = repositoryFactory;
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
            var galaxy = _repositoryFactory.Create(galaxyName);
            galaxy.Login(userName);
            _galaxies.Add(galaxy);
        }
        
        public async Task RegisterGalaxyAsync(string galaxyName, string userName, CancellationToken token)
        {
            if (IsGalaxyRegistered(galaxyName, userName)) return;
            var galaxy = await _repositoryFactory.CreateAsync(galaxyName, token);
            await galaxy.LoginAsync(userName, token);
            _galaxies.Add(galaxy);
        }
        
        public async Task RegisterGalaxiesAsync(string userName, CancellationToken token)
        {
            var galaxies = (await _galaxyFinder.FindAllAsync(token)).ToList();
            var creationTasks = galaxies.Select(g => _repositoryFactory.CreateAsync(g, token)).ToList();

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