using System;
using System.ServiceModel;
using Autofac;
using Autofac.Integration.Wcf;
using GServer.Services;
using NLog;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.HostConfigurators;

namespace GServer.Host.Messaging
{
    // ReSharper disable once ClassNeverInstantiated.Global - Instantiated by Topshelf
    public class MessagingService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private ServiceHost _galaxyManagerHost;
        private ServiceHost _archiveManagerHost;
        private static IContainer _container;

        public static void Main(string[] args)
        {
            //LoggerConfiguration.Apply();
            
            _container = BuildContainer();

            HostFactory.Run(GenerateConfiguration);
        }

        private static void GenerateConfiguration(HostConfigurator config)
        {
            config.UseAutofacContainer(_container);
            config.UseNLog();

            config.Service<MessagingService>(instance =>
            {
                instance.ConstructUsingAutofacContainer();
                instance.WhenStarted(x => x.OnStart());
                instance.WhenStopped(x => x.OnStop());
            });
            
            config.OnException(OnServiceException);
            
            config.SetServiceName("GMessaging");
            config.SetDisplayName("Galaxy Merge Messaging Service");
            config.SetDescription(@"WCF Service host for the Galaxy Merge application. 
                                    This service provides client access for managing, processing,
                                     and retrieving galaxy data.");
            
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
            
            Logger.Trace("Instantiating Archive Manager Service");
            _archiveManagerHost?.Close();
            _archiveManagerHost = new ServiceHost(typeof(ArchiveManager));
            
            Logger.Trace("Configuring Archive Manager Service");
            _archiveManagerHost.AddDependencyInjectionBehavior(typeof(ArchiveManager), _container);

            Logger.Trace("Starting Archive Manager Service");
            _archiveManagerHost.Open();

            Logger.Info("GMessaging Service Started");
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

            _container.Dispose();
            _container = null;
        }

        private static void OnServiceException(Exception obj)
        {
            Logger.Fatal(obj);
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            return builder.Build();
        }
    }
}