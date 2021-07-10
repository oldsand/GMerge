using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace GCommon.Core.Utilities
{
    public static class DbStringBuilder
    {
        public static string GalaxyString(string galaxyName)
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = Environment.MachineName,
                InitialCatalog = galaxyName,
                IntegratedSecurity = true
            }.ConnectionString;
        }
        
        public static string GalaxyString(string hostName, string galaxyName)
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = hostName,
                InitialCatalog = galaxyName,
                IntegratedSecurity = true
            }.ConnectionString;
        }
        
        public static SqlConnectionStringBuilder GalaxyBuilder(string galaxyName)
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = Environment.MachineName,
                InitialCatalog = galaxyName,
                IntegratedSecurity = true
            };
        }
        
        public static SqlConnectionStringBuilder GalaxyBuilder(string hostName, string galaxyName)
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = hostName,
                InitialCatalog = galaxyName,
                IntegratedSecurity = true
            };
        }

        public static string ArchiveString(string galaxyName, string path = null)
        {
            path ??= ApplicationPath.Archives;
            
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            return new SqliteConnectionStringBuilder
            {
                DataSource = Path.Combine(path, $"{galaxyName}.db")
            }.ConnectionString;
        }
        
        public static SqliteConnectionStringBuilder ArchiveBuilder(string galaxyName, string path = null)
        {
            path ??= ApplicationPath.Archives;
            
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            return new SqliteConnectionStringBuilder
            {
                DataSource = Path.Combine(path, $"{galaxyName}.db")
            };
        }
    }
}