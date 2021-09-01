using GServer.Archestra;

namespace GTest.ConsoleApp
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var galaxy = new GalaxyRepository("TestReactor");
            galaxy.Login("ENE\\tnunnink");
            
            const string fileName = @"C:\Users\tnunnink\Desktop\React.xml";
            var manager = new GalaxyFileManager(galaxy);
            
            manager.ExportGraphic("React", fileName);
        }
    }
}