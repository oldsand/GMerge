using System;
using System.ServiceModel;
using Autofac;
using Autofac.Integration.Wcf;
using GalaxyMerge.Archestra;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Services;

namespace GalaxyMerge.Host
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<GalaxyFactory>().As<IGalaxyFactory>();
            builder.RegisterType<GalaxyRegistry>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<GalaxyManager>();
            var container = builder.Build();

            /*var hostGalaxyManager = new ServiceHost(typeof(GalaxyRegistrationManager));
            hostGalaxyManager.AddDependencyInjectionBehavior(typeof(GalaxyRegistrationManager), container);*/

            var hostGalaxyDataManager = new ServiceHost(typeof(GalaxyManager));
            hostGalaxyDataManager.AddDependencyInjectionBehavior(typeof(GalaxyManager), container);

            //Example of procedurally constructing a service endpoint
            /*const string address = "net.tcp://localhost:8009/GalaxyService";
            var binding = new NetTcpBinding();
            var contract = typeof(IGalaxyService);
            hostGalaxyManager.AddServiceEndpoint(contract, binding, address);*/
            
            hostGalaxyDataManager.Open();
            
            Console.WriteLine("Service Started. Press [Enter] to stop service.");
            Console.ReadLine();
            
            hostGalaxyDataManager.Close();
        }
    }
}