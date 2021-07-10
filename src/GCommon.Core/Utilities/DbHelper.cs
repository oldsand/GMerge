using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace GCommon.Core.Utilities
{
    public class DbHelper
    {
        public static bool TryConnect(DbConnection connection)
        {
            try
            {
                connection.Open();
                return connection.State == ConnectionState.Open;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static async Task<bool> TryConnectAsync(DbConnection connection, CancellationToken token)
        {
            try
            {
                await connection.OpenAsync(token).ConfigureAwait(false);
                return connection.State == ConnectionState.Open;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public static int ExecuteCommand(DbConnection connection, string query)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            EnsureOpened(connection);
            return command.ExecuteNonQuery();
        }

        public static async Task<int> ExecuteCommandAsync(DbConnection connection, string query,
            CancellationToken token)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            await EnsureOpenedAsync(connection, token).ConfigureAwait(false);
            return await command.ExecuteNonQueryAsync(token).ConfigureAwait(false);
        }

        public static int ExecuteCommand(DbConnection connection, string query, DbParameter parameter)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(parameter);

            EnsureOpened(connection);
            return command.ExecuteNonQuery();
        }

        public static async Task<int> ExecuteCommandAsync(DbConnection connection, string query, DbParameter parameter,
            CancellationToken token)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(parameter);

            await EnsureOpenedAsync(connection, token).ConfigureAwait(false);
            return await command.ExecuteNonQueryAsync(token).ConfigureAwait(false);
        }

        public static int ExecuteCommand(DbConnection connection, string query, IEnumerable<DbParameter> parameters)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            foreach (var parameter in parameters)
                command.Parameters.Add(parameter);

            EnsureOpened(connection);
            return command.ExecuteNonQuery();
        }

        public static async Task<int> ExecuteCommandAsync(DbConnection connection, string query,
            IEnumerable<DbParameter> parameters, CancellationToken token)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            foreach (var parameter in parameters)
                command.Parameters.Add(parameter);

            await EnsureOpenedAsync(connection, token).ConfigureAwait(false);
            return await command.ExecuteNonQueryAsync(token).ConfigureAwait(false);
        }

        public static object ExecuteScalar(DbConnection connection, string query)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            EnsureOpened(connection);
            return command.ExecuteScalar();
        }

        public static async Task<object> ExecuteScalarAsync(DbConnection connection, string query,
            CancellationToken token)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            await EnsureOpenedAsync(connection, token).ConfigureAwait(false);
            return await command.ExecuteScalarAsync(token).ConfigureAwait(false);
        }

        public static object ExecuteScalar(DbConnection connection, string query, DbParameter parameter)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(parameter);

            EnsureOpened(connection);
            return command.ExecuteScalar();
        }

        public static async Task<object> ExecuteScalarAsync(DbConnection connection, string query,
            DbParameter parameter, CancellationToken token)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.Add(parameter);

            await EnsureOpenedAsync(connection, token).ConfigureAwait(false);
            return await command.ExecuteScalarAsync(token).ConfigureAwait(false);
        }

        public static object ExecuteScalar(DbConnection connection, string query, IEnumerable<DbParameter> parameters)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            foreach (var parameter in parameters)
                command.Parameters.Add(parameter);

            EnsureOpened(connection);
            return command.ExecuteScalar();
        }

        public static async Task<object> ExecuteScalarAsync(DbConnection connection, string query,
            IEnumerable<DbParameter> parameters, CancellationToken token)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            foreach (var parameter in parameters)
                command.Parameters.Add(parameter);

            await EnsureOpenedAsync(connection, token).ConfigureAwait(false);
            return await command.ExecuteScalarAsync(token).ConfigureAwait(false);
        }

        public static IDataReader ExecuteReader(DbConnection connection, string commandText)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;

            EnsureOpened(connection);
            return command.ExecuteReader();
        }

        public static async Task<IDataReader> ExecuteReaderAsync(DbConnection connection, string commandText,
            CancellationToken token)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;

            await EnsureOpenedAsync(connection, token).ConfigureAwait(false);
            return await command.ExecuteReaderAsync(token).ConfigureAwait(false);
        }

        public static IDataReader ExecuteReader(DbConnection connection, string commandText, DbParameter parameter)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.Parameters.Add(parameter);

            EnsureOpened(connection);
            return command.ExecuteReader();
        }

        public static async Task<IDataReader> ExecuteReaderAsync(DbConnection connection, string commandText,
            DbParameter parameter, CancellationToken token)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.Parameters.Add(parameter);

            await EnsureOpenedAsync(connection, token).ConfigureAwait(false);
            return await command.ExecuteReaderAsync(token).ConfigureAwait(false);
        }

        public static IDataReader ExecuteReader(DbConnection connection, string commandText,
            IEnumerable<DbParameter> parameters)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;

            foreach (var parameter in parameters)
                command.Parameters.Add(parameter);

            EnsureOpened(connection);
            return command.ExecuteReader();
        }

        public static async Task<IDataReader> ExecuteReaderAsync(DbConnection connection, string commandText,
            IEnumerable<DbParameter> parameters, CancellationToken token)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;

            foreach (var parameter in parameters)
                command.Parameters.Add(parameter);

            await EnsureOpenedAsync(connection, token).ConfigureAwait(false);
            return await command.ExecuteReaderAsync(token).ConfigureAwait(false);
        }

        private static void EnsureOpened(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }

        private static async Task EnsureOpenedAsync(DbConnection connection, CancellationToken token)
        {
            if (connection.State != ConnectionState.Open) await connection.OpenAsync(token).ConfigureAwait(false);
        }
    }
}