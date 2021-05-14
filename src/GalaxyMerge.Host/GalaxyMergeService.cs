using System.ServiceModel;
using Autofac;
using Autofac.Integration.Wcf;
using GalaxyMerge.Services;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.HostConfigurators;
// ReSharper disable ClassNeverInstantiated.Global

namespace GalaxyMerge.Host
{
    public class GalaxyMergeService
    {
        private ServiceHost _galaxyManagerHost;
        private ServiceHost _archiveManagerHost;
        private ArchiveController _archiveController;
        private static IContainer _container;

        public static void Main(string[] args)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap();
            _container = bootstrapper.GetContainer();

            HostFactory.Run(ServiceConfiguration);
        }

        private static void ServiceConfiguration(HostConfigurator config)
        {
            config.UseAutofacContainer(_container);
            
            config.Service<GalaxyMergeService>(instance =>
            {
                instance.ConstructUsingAutofacContainer();
                instance.WhenStarted(x => x.OnStart());
                instance.WhenStopped(x => x.OnStop());
            });
            
            config.SetServiceName("gmerge");
            config.SetDisplayName("Galaxy Merge");
            config.SetDescription(@"Service host for the Galaxy Merge application.
                                  This service provides retrieval, archiving, and modification of System Platform
                                  Archestra objects and graphics hosted on this machine's galaxy repositories.");
            config.StartAutomatically();
        }

        private void OnStart()
        {
            _galaxyManagerHost?.Close();
            _galaxyManagerHost = new ServiceHost(typeof(GalaxyManager));
            _galaxyManagerHost.AddDependencyInjectionBehavior(typeof(GalaxyManager), _container);
            _galaxyManagerHost.Open();
            
            _archiveManagerHost?.Close();
            _archiveManagerHost = new ServiceHost(typeof(ArchiveManager));
            _archiveManagerHost.AddDependencyInjectionBehavior(typeof(ArchiveManager), _container);
            _archiveManagerHost.Open();

            _archiveController?.Stop();
            _archiveController = new ArchiveController(_container.Resolve<IGalaxyRepositoryProvider>());
            _archiveController.Start();
        }
        
        private void OnStop()
        {
            if (_galaxyManagerHost == null) return;
            _galaxyManagerHost.Close();
            _galaxyManagerHost = null;
            
            if (_archiveManagerHost == null) return;
            _archiveManagerHost.Close();
            _archiveManagerHost = null;
            
            if (_archiveController == null) return;
            _archiveController.Stop();
            _archiveController = null;
            
            _container.Dispose();
            _container = null;
        }
    }
}