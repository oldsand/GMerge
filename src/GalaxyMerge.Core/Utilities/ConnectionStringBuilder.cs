using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace GalaxyMerge.Core.Utilities
{
    public static class ConnectionStringBuilder
    {
        public static string BuildGalaxyConnection(string galaxyName)
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = Environment.MachineName,
                InitialCatalog = galaxyName,
                IntegratedSecurity = true
            }.ConnectionString;
        }

        public static string BuildArchiveConnection(string galaxyName)
        {
            if (!Directory.Exists(ApplicationPath.Archives))
                Directory.CreateDirectory(ApplicationPath.Archives);
            
            return new SqliteConnectionStringBuilder()
            {
                DataSource = Path.Combine(ApplicationPath.Archives, $"{galaxyName}.db")
            }.ConnectionString;
        }
    }
}