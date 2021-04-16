using System;
using GalaxyMerge.Archestra;
using GalaxyMerge.Services;

namespace TestConsole
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var service = new GalaxyArchiveService(new GalaxyFinder(), new GalaxyRegistry());
            service.Start();

            Console.WriteLine("Press Key to stop");
            Console.ReadLine();

            service.Stop();
        }
    }
}