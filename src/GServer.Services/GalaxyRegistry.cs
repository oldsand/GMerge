using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using GServer.Archestra;
using GServer.Archestra.Abstractions;
using GServer.Services.Abstractions;
using NLog;

namespace GServer.Services
{
    public class GalaxyRegistry : IGalaxyRegistry
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IGalaxyRepositoryFactory _repositoryFactory;
        private readonly List<IGalaxyRepository> _galaxies = new();

        public GalaxyRegistry()
        {
            _repositoryFactory = new GalaxyRepositoryFactory();
        }

        internal GalaxyRegistry(IGalaxyRepositoryFactory galaxyRepositoryFactory)
        {
            _repositoryFactory = galaxyRepositoryFactory;
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

        public IEnumerable<IGalaxyRepository> GetByCurrentIdentity()
        {
            var userName = WindowsIdentity.GetCurrent().Name;
            return _galaxies.Where(g => g.ConnectedUser == userName);
        }

        public IGalaxyRepository GetByCurrentIdentity(string galaxyName)
        {
            var userName = WindowsIdentity.GetCurrent().Name;
            return _galaxies.SingleOrDefault(g => g.Name == galaxyName && g.ConnectedUser == userName);
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
            var galaxyRepositories = _repositoryFactory.CreateAll();

            Parallel.ForEach(galaxyRepositories, galaxyRepository =>
            {
                if (IsRegistered(galaxyRepository.Name, user.Name)) return;

                galaxyRepository.Login(user.Name);
                _galaxies.Add(galaxyRepository);
                
                Logger.Debug("Galaxy {Galaxy} successfully registered to {User}", galaxyRepository.Name, user.Name);
            });
        }

        public void RegisterAll(string userName)
        {
            var galaxyRepositories = _repositoryFactory.CreateAll();

            Parallel.ForEach(galaxyRepositories, galaxyRepository =>
            {
                if (IsRegistered(galaxyRepository.Name, userName)) return;

                galaxyRepository.Login(userName);
                _galaxies.Add(galaxyRepository);
                
                Logger.Debug("Galaxy {Galaxy} successfully registered to {User}", galaxyRepository.Name, userName);
            });
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
            
            Logger.Trace("Registering galaxy {Galaxy} to user {User}", galaxyName, userName);
            
            if (IsRegistered(galaxyName, userName))
            {
                Logger.Trace("Galaxy {Galaxy} already registered to user {User}", galaxyName, userName);
                return;
            }
            
            var galaxy = _repositoryFactory.Create(galaxyName);
            galaxy.Login(userName);
            _galaxies.Add(galaxy);
            
            Logger.Debug("Galaxy {Galaxy} successfully registered to {User}", galaxyName, userName);
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