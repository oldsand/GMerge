using System;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Entities;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace GalaxyMerge.Services
{
    public class GalaxyWatcher : IDisposable
    {
        private SqlTableDependency<ChangeLog> _dependency;
        private readonly ArchiveQueue _archiveQueue;
        private const string ChangeLogTableName = "gobject_change_log";

        public GalaxyWatcher(IGalaxyRepository galaxyRepository)
        {
            SetupServiceBroker(galaxyRepository.Name);
            InitializeDependency(galaxyRepository.Name);
            _archiveQueue = new ArchiveQueue(galaxyRepository);
        }

        private static void SetupServiceBroker(string databaseName)
        {
            if (!SqlServiceBroker.IsUnique(databaseName))
            {
                SqlServiceBroker.New(databaseName);
                return;
            }
            
            if (!SqlServiceBroker.IsEnabled(databaseName))
                SqlServiceBroker.Enable(databaseName);
        }
        
        private void InitializeDependency(string databaseName)
        {
            var connectionString = DbStringBuilder.BuildGalaxy(databaseName);
            _dependency = new SqlTableDependency<ChangeLog>(connectionString, ChangeLogTableName,
                mapper: DataMapper.GetChangeLogMapper(), executeUserPermissionCheck: false);
            _dependency.OnChanged += OnChangeLogTableUpdated;
            _dependency.Start();
        }
        
        private void OnChangeLogTableUpdated(object sender, RecordChangedEventArgs<ChangeLog> e)
        {
            if (e.ChangeType != ChangeType.Insert) return;
            var changeLog = e.Entity;
            _archiveQueue.Enqueue(changeLog);
        }

        public void Dispose()
        {
            _dependency?.Stop();
            _dependency?.Dispose();
        }
    }
}