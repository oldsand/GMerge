using System;
using Autofac;
using GalaxyMerge.Archestra;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive;
using GalaxyMerge.Archive.Abstractions;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Services;

namespace GalaxyMerge.Host
{
    public class Bootstrapper
    {
        private IContainer _container;
        
        public void Bootstrap()
        {
            ConfigureContainer();
            RegisterGalaxies();
            EnsureArchivesExist();
        }

        public IContainer GetContainer()
        {
            return _container;
        }

        private void ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<GalaxyRepositoryFactory>().As<IGalaxyRepositoryFactory>();
            builder.RegisterType<GalaxyRegistry>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GalaxyRepositoryProvider>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<GalaxyManager>();
            builder.RegisterType<ArchiveBuilder>().AsSelf().AsImplementedInterfaces();
            _container = builder.Build();
        }

        private void RegisterGalaxies()
        {
            if (_container == null)
                throw new InvalidOperationException("Container not yet initialized");

            var registry = _container.Resolve<IGalaxyRegistry>();
            
            registry.RegisterAll(); //todo should this be async/parallel?
        }

        private void EnsureArchivesExist()
        {
            if (_container == null)
                throw new InvalidOperationException("Container not yet initialized");

            var builder = _container.Resolve<IArchiveBuilder>();
            var registry = _container.Resolve<IGalaxyRegistry>();
            var galaxies = registry.GetAll();

            foreach (var galaxy in galaxies)
                builder.Build(ArchiveConfigurationBuilder.Default(galaxy.Name, galaxy.VersionNumber, galaxy.CdiVersion,
                    galaxy.VersionString));
        }
    }
}