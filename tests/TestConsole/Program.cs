using System;
using GalaxyMerge.Archestra;
using GalaxyMerge.Services;

namespace TestConsole
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var service = new GalaxyRegistrationService(new GalaxyFinder(), new GalaxyRegistry());
            service.Start();
            Console.WriteLine("Press and Key to stop");
            Console.ReadLine();
            service.Stop();

            /*var service = new GalaxyArchiveService(new GalaxyRegistry());
            service.Start();

            Console.WriteLine("Press Key to stop");
            Console.ReadLine();

            service.Stop();*/
        }
    }
}