using System;
using System.Diagnostics;
using Autofac;
using GalaxyMerge.Archive;
using GalaxyMerge.Archive.Abstractions;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Services;
using NLog;

namespace GalaxyMerge.Host
{
    public class Bootstrapper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IContainer _container;
        
        public void Bootstrap()
        {
            Logger.Debug("Running Service Bootstrapper");
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
            Logger.Trace("Configuring DI Container");
            var builder = new ContainerBuilder();
            builder.RegisterType<GalaxyMergeService>().AsSelf();
            builder.RegisterType<GalaxyRegistry>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GalaxyRepositoryProvider>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<GalaxyDataRepositoryFactory>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<GalaxyManager>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<ArchiveManager>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<ArchiveBuilder>().AsSelf().AsImplementedInterfaces();
            _container = builder.Build();
        }

        private void RegisterGalaxies()
        {
            Logger.Trace("Registering Galaxies");
            if (_container == null)
                throw new InvalidOperationException("Container not yet initialized");

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                
                var registry = _container.Resolve<IGalaxyRegistry>();
                registry.RegisterParallel();
                
                stopwatch.Stop();
                Logger.Debug("Galaxy registration complete. Ellapsed time: {Time}", stopwatch.Elapsed);
            }
            catch (Exception)
            {
                Logger.Error("Registration failed");
                throw;
            }
        }

        private void EnsureArchivesExist()
        {
            Logger.Trace("Building Archives");
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