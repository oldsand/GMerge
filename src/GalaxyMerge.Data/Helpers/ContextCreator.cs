using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Helpers
{
    internal static class ContextCreator
    {
        public static GalaxyContext Create(string connectionString)
        {
            var options = new DbContextOptionsBuilder<GalaxyContext>().UseSqlServer(connectionString).Options;
            return new GalaxyContext(options);
        }
    }
}