using System;
using System.Collections.Generic;
using System.ServiceModel;
using Autofac;
using Autofac.Integration.Wcf;
using GalaxyMerge.Host.Configurations;
using GalaxyMerge.Services;
using NLog;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.HostConfigurators;

// ReSharper disable ClassNeverInstantiated.Global

namespace GalaxyMerge.Host
{
    public class GalaxyMergeService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private ServiceHost _galaxyManagerHost;
        private ServiceHost _archiveManagerHost;
        private readonly List<GalaxyWatcher> _galaxyWatchers = new List<GalaxyWatcher>();
        private static IContainer _container;

        public static void Main(string[] args)
        {
            LoggerConfiguration.Apply();

            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap();
            _container = bootstrapper.GetContainer();

            HostFactory.Run(GalaxyMergeConfiguration);
        }

        private static void GalaxyMergeConfiguration(HostConfigurator config)
        {
            config.UseAutofacContainer(_container);
            config.UseNLog();

            config.Service<GalaxyMergeService>(instance =>
            {
                instance.ConstructUsingAutofacContainer();
                instance.WhenStarted(x => x.OnStart());
                instance.WhenStopped(x => x.OnStop());
            });
            
            config.OnException(OnServiceException);
            
            config.SetServiceName("gmerge");
            config.SetDisplayName("Galaxy Merge");
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
            _galaxyManagerHost?.Close();
            _galaxyManagerHost = new ServiceHost(typeof(GalaxyManager));
            _galaxyManagerHost.AddDependencyInjectionBehavior(typeof(GalaxyManager), _container);
            Logger.Debug("Starting Galaxy Manager Service");
            _galaxyManagerHost.Open();
            
            _archiveManagerHost?.Close();
            _archiveManagerHost = new ServiceHost(typeof(ArchiveManager));
            _archiveManagerHost.AddDependencyInjectionBehavior(typeof(ArchiveManager), _container);
            Logger.Debug("Starting Archive Manager Service");
            _archiveManagerHost.Open();
            
            var provider = _container.Resolve<IGalaxyRepositoryProvider>();
            var serviceGrSessions = provider.GetAllServiceInstances();
            Logger.Debug("Starting Galaxy Watcher Service(s)");
            foreach (var gr in serviceGrSessions)
                _galaxyWatchers.Add(new GalaxyWatcher(gr));
            
            Logger.Info("GalaxyMerge Service Started");
        }
        
        private void OnStop()
        {
            if (_galaxyManagerHost != null)
            {
                _galaxyManagerHost.Close();
                _galaxyManagerHost = null;    
            }
            
            if (_archiveManagerHost != null)
            {
                _archiveManagerHost.Close();
                _archiveManagerHost = null;    
            }

            if (_galaxyWatchers.Count > 0)
            {
                foreach (var watcher in _galaxyWatchers)
                    watcher.Dispose();

                _galaxyWatchers.Clear();
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