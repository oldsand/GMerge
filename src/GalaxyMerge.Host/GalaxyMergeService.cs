using System.ServiceModel;
using System.ServiceProcess;
using Autofac.Integration.Wcf;
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
        
        protected override void OnStart(string[] args)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Bootstrap();
            var container = bootstrapper.GetContainer();
            
            //TODO: Another thing to think about will be host configuration. Would like to do it programatically. 
            _galaxyManagerHost?.Close();
            _galaxyManagerHost = new ServiceHost(typeof(GalaxyManager));
            _galaxyManagerHost.AddDependencyInjectionBehavior(typeof(GalaxyManager), container);
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