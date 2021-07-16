using System;
using System.Data;
using System.Data.SqlClient;

namespace GTest.ConsoleApp
{
    public class SqlNotifier : IDisposable
    {
        public SqlCommand CurrentCommand { get; set; }
        private SqlConnection _connection;

        public SqlConnection CurrentConnection
        {
            get
            {
                _connection ??= new SqlConnection(ConnectionString);
                return _connection;
            }
        }

        public string ConnectionString { get; }

        public SqlNotifier(string database)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = Environment.MachineName,
                InitialCatalog = database,
                IntegratedSecurity = true
            };

            ConnectionString = builder.ConnectionString;

            SqlDependency.Start(ConnectionString);
        }

        public event EventHandler<SqlNotificationEventArgs> TableChanged;

        private void OnTableChanged(SqlNotificationEventArgs notification)
        {
            TableChanged?.Invoke(this, notification);
        }

        public DataTable RegisterDependency()
        {
            const string query = "SELECT gobject_id from dbo.gobject_change_log WHERE operation_id in (0, 14, 30, 38)";
            CurrentCommand = new SqlCommand(query, CurrentConnection) {Notification = null};

            var dependency = new SqlDependency(CurrentCommand);
            dependency.OnChange += OnDependencyChanged;

            if (CurrentConnection.State == ConnectionState.Closed) CurrentConnection.Open();
            
            var dt = new DataTable();
            dt.Load(CurrentCommand.ExecuteReader
                (CommandBehavior.CloseConnection));
            return dt;
        }

        private void OnDependencyChanged(object sender, SqlNotificationEventArgs e)
        {
            if (sender is SqlDependency dependency)
                dependency.OnChange -= OnDependencyChanged;

            OnTableChanged(e);
        }

        public void Dispose()
        {
            SqlDependency.Stop(ConnectionString);
        }
    }
}