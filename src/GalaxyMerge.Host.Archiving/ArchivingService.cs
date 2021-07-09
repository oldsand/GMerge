using System;
using System.Collections.Generic;
using System.ServiceModel;
using Autofac;
using Autofac.Integration.Wcf;
using GalaxyMerge.Host.Configurations;
using GalaxyMerge.Services;
using GalaxyMerge.Services.Abstractions;
using NLog;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.HostConfigurators;

// ReSharper disable ClassNeverInstantiated.Global

namespace GalaxyMerge.Host.Archiving
{
    public class ArchivingService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly List<ArchiveMonitor> _archiveMonitors = new();
        private static IContainer _container;

        public static void Main(string[] args)
        {
            LoggerConfiguration.Apply();

            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap();
            _container = bootstrapper.GetContainer();

            HostFactory.Run(GenerateConfiguration);
        }

        private static void GenerateConfiguration(HostConfigurator config)
        {
            config.UseAutofacContainer(_container);
            config.UseNLog();

            config.Service<ArchivingService>(instance =>
            {
                instance.ConstructUsingAutofacContainer();
                instance.WhenStarted(x => x.OnStart());
                instance.WhenStopped(x => x.OnStop());
            });
            
            config.OnException(OnServiceException);
            
            config.SetServiceName("GArchiving");
            config.SetDisplayName("Galaxy Merge Archiving Service");
            config.SetDescription(@"Service host for the Galaxy Merge application.
                                  This service provides retrieval, archiving, and modification of System Platform
                                  Archestra objects and graphics hosted on this machine's galaxy repositories.");
            
            config.SetStartTimeout(TimeSpan.FromSeconds(60));
            config.SetStopTimeout(TimeSpan.FromSeconds(30));
            config.RunAsLocalSystem();
            config.StartAutomatically();
        }

        private void OnStart()
        {
            var registry = _container.Resolve<IGalaxyRegistry>();
            var galaxyRepositories = registry.GetByCurrentIdentity();
            
            foreach (var galaxyRepository in galaxyRepositories)
            {
                var monitor = new ArchiveMonitor(galaxyRepository.Name);
                _archiveMonitors.Add(monitor);
            }

            Logger.Info("GalaxyMerge Service Started");
        }
        
        private void OnStop()
        {
            if (_archiveMonitors.Count > 0)
            {
                foreach (var monitor in _archiveMonitors)
                    monitor.Dispose();

                _archiveMonitors.Clear();
            }
            
            _container.Dispose();
            _container = null;
        }

        private static void OnServiceException(Exception obj)
        {
            Logger.Fatal(obj);
        }
    }
}