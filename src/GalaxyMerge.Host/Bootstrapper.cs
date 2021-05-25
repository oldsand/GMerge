using System;
using Autofac;
using GalaxyMerge.Archive;
using GalaxyMerge.Archive.Abstractions;
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
            builder.RegisterType<GalaxyMergeService>().AsSelf();
            builder.RegisterType<GalaxyRegistry>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GalaxyRepositoryProvider>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<GalaxyManager>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<ArchiveManager>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<ArchiveBuilder>().AsSelf().AsImplementedInterfaces();
            _container = builder.Build();
        }

        private void RegisterGalaxies()
        {
            if (_container == null)
                throw new InvalidOperationException("Container not yet initialized");

            try
            {
                var registry = _container.Resolve<IGalaxyRegistry>();
                registry.RegisterParallel();
                Console.WriteLine("Yay");
                //todo probably just log here
            }
            catch (Exception e)
            {
                Console.WriteLine("Well shit...");
                //todo log errors to event logs, ensure that the service does not start.
                Console.WriteLine(e);
                throw;
            }
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