using System;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GalaxyMerge.Test.Core
{
    public class SqliteTestFixture<T> : DbTestFixture<T>, IDisposable where T : DbContext
    {
        protected readonly DbConnection Connection;

        protected SqliteTestFixture()
            : base(
                new DbContextOptionsBuilder<T>()
                    .UseSqlite(CreateInMemoryDatabase())
                    .Options)
        {
            Connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
        }

        protected void Create(T context)
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
            
            context.Database.EnsureCreated();
        }
        
        public void Dispose() => Connection.Dispose();

        private static DbConnection CreateInMemoryDatabase()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:"};
            
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            connection.Open();
            
            return connection;
        }
    }
}