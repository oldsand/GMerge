using System;
using System.ServiceModel;
using System.ServiceProcess;
using Autofac;
using Autofac.Integration.Wcf;
using GalaxyMerge.Archestra;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Services;

namespace GalaxyMerge.Host
{
    public class GalaxyMergeService : ServiceBase
    {
        private ServiceHost _galaxyManagerHost;

        private GalaxyMergeService()
        {
            ServiceName = "GalaxyMerge";
        }
        
        public static void Main()
        {
            Run(new GalaxyMergeService());
        }
        
        // Start the Windows service.
        protected override void OnStart(string[] args)
        {
            _galaxyManagerHost?.Close();

            //Setup DI Container.
            var builder = new ContainerBuilder();
            builder.RegisterType<GalaxyFactory>().As<IGalaxyFactory>();
            builder.RegisterType<GalaxyRegistry>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GalaxyManager>();
            var container = builder.Build();

            // Create a ServiceHost for the GalaxyManager type and
            // provide the base address.
            _galaxyManagerHost = new ServiceHost(typeof(GalaxyManager));
            _galaxyManagerHost.AddDependencyInjectionBehavior(typeof(GalaxyManager), container);

            // Open the ServiceHostBase to create listeners and start
            // listening for messages.
            _galaxyManagerHost.Open();
        }
        
        protected override void OnStop()
        {
            if (_galaxyManagerHost == null) return;
            _galaxyManagerHost.Close();
            _galaxyManagerHost = null;
        }
    }
}