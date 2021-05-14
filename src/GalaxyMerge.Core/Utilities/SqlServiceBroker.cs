using Microsoft.Data.SqlClient;

namespace GalaxyMerge.Core.Utilities
{
    public static class SqlServiceBroker
    {
        public static bool IsEnabled(string databaseName)
        {
            var connectionString = ConnectionStringBuilder.BuildGalaxyConnection(databaseName);
            var query = $"select is_broker_enabled from sys.databases where name = '{databaseName}'";
            return (bool) DbHelper.ExecuteScalar(new SqlConnection(connectionString), query);
        }
        
        public static void Enable(string databaseName)
        {
            var connectionString = ConnectionStringBuilder.BuildGalaxyConnection(databaseName);
            var statement = $"ALTER DATABASE {databaseName} SET ENABLE_BROKER with rollback immediate";
            DbHelper.ExecuteCommand(new SqlConnection(connectionString), statement);
        }
        
        public static void Disable(string databaseName)
        {
            var connectionString = ConnectionStringBuilder.BuildGalaxyConnection(databaseName);
            var statement = $"ALTER DATABASE {databaseName} SET DISABLE_BROKER with rollback immediate";
            DbHelper.ExecuteCommand(new SqlConnection(connectionString), statement);
        }
    }
}