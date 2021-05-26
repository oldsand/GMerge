using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

        public bool IsRegistered(string galaxyName, string userName)
        {
            return _galaxies.SingleOrDefault(g => g.Name == galaxyName && g.ConnectedUser == userName) != null;
        }
        
        public IGalaxyRepository GetGalaxy(string galaxyName, string userName)
        {
            return _galaxies.SingleOrDefault(g => g.Name == galaxyName && g.ConnectedUser == userName);
        }

        public IEnumerable<IGalaxyRepository> GetByName(string galaxyName)
        {
            return _galaxies.Where(g => g.Name == galaxyName);
        }

        public IEnumerable<IGalaxyRepository> GetByUser(string userName)
        {
            return _galaxies.Where(g => g.ConnectedUser == userName);
        }

        public IEnumerable<IGalaxyRepository> GetAll()
        {
            return _galaxies;
        }

        public void Register(string galaxyName)
        {
            var user = WindowsIdentity.GetCurrent();
            RegisterGalaxy(galaxyName, user.Name);
        }

        public void Register(string galaxyName, string userName)
        {
            RegisterGalaxy(galaxyName, userName);
        }
        
        public void RegisterAll()
        {
            var user = WindowsIdentity.GetCurrent();
            var galaxies = _galaxyFinder.FindAll();
            
            foreach (var galaxy in galaxies)
                RegisterGalaxy(galaxy, user.Name);
        }

        public void RegisterAll(string userName)
        {
            var galaxies = _galaxyFinder.FindAll();
            foreach (var galaxy in galaxies)
                RegisterGalaxy(galaxy, userName);
        }
        
        public void RegisterParallel()
        {
            var user = WindowsIdentity.GetCurrent();
            var galaxyRepositories = _repositoryFactory.CreateAll();

            Parallel.ForEach(galaxyRepositories, galaxyRepository =>
            {
                if (IsRegistered(galaxyRepository.Name, user.Name))
                {
                    galaxyRepository.SynchronizeClient();
                    return;
                }
                
                galaxyRepository.Login(user.Name);
                _galaxies.Add(galaxyRepository);
            });
        }

        public Task RegisterAsync(string galaxyName, CancellationToken token)
        {
            var user = WindowsIdentity.GetCurrent();
            return RegisterGalaxyAsync(galaxyName, user.Name, token);
        }

        public Task RegisterAsync(string galaxyName, string userName, CancellationToken token)
        {
            return RegisterGalaxyAsync(galaxyName, userName, token);
        }

        public async Task RegisterAllAsync(CancellationToken token)
        {
            var user = WindowsIdentity.GetCurrent();
            var unregisteredGalaxies = (await _galaxyFinder.FindAllAsync(token))
                .Where(g => !IsRegistered(g, user.Name)).ToList();
            var creationTasks = unregisteredGalaxies.Select(g => _repositoryFactory.CreateAsync(g, token)).ToList();

            while (creationTasks.Any())
            {
                var connection = await Task.WhenAny(creationTasks);
                creationTasks.Remove(connection);
                var galaxy = connection.Result;
                await galaxy.LoginAsync(user.Name, token);
                _galaxies.Add(galaxy);
            }
        }

        public async Task RegisterAllAsync(string userName, CancellationToken token)
        {
            var galaxies = (await _galaxyFinder.FindAllAsync(token))
                .Where(g => !IsRegistered(g, userName)).ToList();
            var creationTasks = galaxies.Select(g => _repositoryFactory.CreateAsync(g, token)).ToList();

            while (creationTasks.Any())
            {
                var connection = await Task.WhenAny(creationTasks);
                creationTasks.Remove(connection);
                var galaxy = connection.Result;
                await galaxy.LoginAsync(userName, token);
                _galaxies.Add(galaxy);
            }
        }

        public void Unregister(string galaxyName, string userName)
        {
            UnregisterGalaxy(galaxyName, userName);
        }

        public void Unregister(IEnumerable<string> galaxies, string userName)
        {
            foreach (var galaxy in galaxies)
                UnregisterGalaxy(galaxy, userName);
        }

        private void RegisterGalaxy(string galaxyName, string userName)
        {
            if (string.IsNullOrEmpty(galaxyName))
                throw new ArgumentException("Value cannot be null or empty", nameof(galaxyName));

            if (userName == null)
                throw new ArgumentException("Value cannot be null", nameof(userName));

            if (IsRegistered(galaxyName, userName)) return;

            var galaxy = _repositoryFactory.Create(galaxyName);
            galaxy.Login(userName);
            _galaxies.Add(galaxy);
        }

        private async Task RegisterGalaxyAsync(string galaxyName, string userName, CancellationToken token)
        {
            if (string.IsNullOrEmpty(galaxyName))
                throw new ArgumentException("Value cannot be null or empty", nameof(galaxyName));

            if (userName == null)
                throw new ArgumentException("Value cannot be null", nameof(userName));

            if (IsRegistered(galaxyName, userName)) return;

            var galaxy = await _repositoryFactory.CreateAsync(galaxyName, token);
            await galaxy.LoginAsync(userName, token);
            _galaxies.Add(galaxy);
        }

        private void UnregisterGalaxy(string galaxyName, string userName)
        {
            if (string.IsNullOrEmpty(galaxyName))
                throw new ArgumentException("Value cannot be null or empty", nameof(galaxyName));

            if (userName == null)
                throw new ArgumentException("Value cannot be null", nameof(userName));

            if (!IsRegistered(galaxyName, userName)) return;

            var galaxy = GetGalaxy(galaxyName, userName);
            _galaxies.Remove(galaxy);
            galaxy.Logout();
        }
    }
}