using Microsoft.Data.SqlClient;

namespace GalaxyMerge.Core.Utilities
{
    public static class SqlServiceBroker
    {
        public static bool IsEnabled(string databaseName)
        {
            var connectionString = DbStringBuilder.GalaxyString(databaseName);
            var query = $"select is_broker_enabled from sys.databases where name = '{databaseName}'";
            return (bool) DbHelper.ExecuteScalar(new SqlConnection(connectionString), query);
        }
        
        public static bool IsUnique(string databaseName)
        {
            var connectionString = DbStringBuilder.GalaxyString(databaseName);
            var query = @$"select count(service_broker_guid) from sys.databases
                           where service_broker_guid = 
                           (select service_broker_guid from sys.databases where name = '{databaseName}')";
            return (int) DbHelper.ExecuteScalar(new SqlConnection(connectionString), query) == 1;
        }

        public static void Enable(string databaseName)
        {
            var connectionString = DbStringBuilder.GalaxyString(databaseName);
            var statement = $"ALTER DATABASE {databaseName} SET ENABLE_BROKER with rollback immediate";
            DbHelper.ExecuteCommand(new SqlConnection(connectionString), statement);
        }
        
        public static void Disable(string databaseName)
        {
            var connectionString = DbStringBuilder.GalaxyString(databaseName);
            var statement = $"ALTER DATABASE {databaseName} SET DISABLE_BROKER with rollback immediate";
            DbHelper.ExecuteCommand(new SqlConnection(connectionString), statement);
        }
        
        public static void New(string databaseName)
        {
            var connectionString = DbStringBuilder.GalaxyString(databaseName);
            var statement = $"ALTER DATABASE {databaseName} SET NEW_BROKER with rollback immediate";
            DbHelper.ExecuteCommand(new SqlConnection(connectionString), statement);
        }
    }
}