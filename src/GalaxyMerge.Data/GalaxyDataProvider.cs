using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data
{
    public class GalaxyDataProvider : IGalaxyDataProvider
    {
        private readonly GalaxyContext _context;

        public GalaxyDataProvider(string connectionString)
        {
            var options = new DbContextOptionsBuilder<GalaxyContext>().UseSqlServer(connectionString).Options;
            _context = new GalaxyContext(options);
            InitializeRepositories(_context);
        }

        public GalaxyDataProvider(string hostName, string galaxyName)
        {
            var connectionString = DbStringBuilder.GalaxyString(hostName, galaxyName);
            var options = new DbContextOptionsBuilder<GalaxyContext>().UseSqlServer(connectionString).Options;
            _context = new GalaxyContext(options);
            InitializeRepositories(_context);
        }

        private void InitializeRepositories(GalaxyContext context)
        {
            Objects = new ObjectRepository(context);
            Definitions = new DefinitionRepository(context);
            Folders = new FolderRepository(context);
            ChangeLogs = new ChangeLogRepository(context);
            Users = new UserRepository(context);
            Lookup = new LookupRepository(context);
        }

        public IObjectRepository Objects { get; private set; }

        public IDefinitionRepository Definitions { get; private set; }

        public IFolderRepository Folders { get; private set; }

        public IChangeLogRepository ChangeLogs { get; private set; }

        public IUserRepository Users { get; private set; }

        public ILookupRepository Lookup { get; private set; }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}