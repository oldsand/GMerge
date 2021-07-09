using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Repositories;
using Microsoft.Data.SqlClient;
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
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = hostName,
                InitialCatalog = galaxyName,
                IntegratedSecurity = true
            }.ConnectionString;
            
            var options = new DbContextOptionsBuilder<GalaxyContext>().UseSqlServer(connectionString).Options;
            _context = new GalaxyContext(options);
            InitializeRepositories(_context);
        }

        private void InitializeRepositories(GalaxyContext context)
        {
            ObjectsReadOnly = new ObjectReadOnlyRepository(context);
            DefinitionsReadOnly = new DefinitionReadOnlyRepository(context);
            FoldersReadOnly = new FolderReadOnlyRepository(context);
            ChangeLogsReadOnly = new ChangeLogReadOnlyRepository(context);
            UsersReadOnly = new UserReadOnlyRepository(context);
            Lookup = new LookupRepository(context);
        }

        public IObjectReadOnlyRepository ObjectsReadOnly { get; private set; }

        public IDefinitionReadOnlyRepository DefinitionsReadOnly { get; private set; }

        public IFolderReadOnlyRepository FoldersReadOnly { get; private set; }

        public IChangeLogReadOnlyRepository ChangeLogsReadOnly { get; private set; }

        public IUserReadOnlyRepository UsersReadOnly { get; private set; }

        public ILookupRepository Lookup { get; private set; }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}