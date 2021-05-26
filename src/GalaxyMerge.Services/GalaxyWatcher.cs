using System;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Entities;
using NLog;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace GalaxyMerge.Services
{
    public class GalaxyWatcher : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private SqlTableDependency<ChangeLog> _changeLogDependency;
        private readonly ArchiveQueue _archiveQueue;
        private const string ChangeLogTableName = "gobject_change_log";

        public GalaxyWatcher(IGalaxyRepository galaxyRepository)
        {
            Logger.Trace("Constructing Galaxy Watcher service instance for '{Galaxy}'", galaxyRepository.Name);
            SetupServiceBroker(galaxyRepository.Name);
            InitializeDependency(galaxyRepository.Name);
            _archiveQueue = new ArchiveQueue(galaxyRepository);
        }

        private static void SetupServiceBroker(string databaseName)
        {
            if (!SqlServiceBroker.IsUnique(databaseName))
            {
                Logger.Trace("Creating new Service Broker for galaxy database '{Database}'", databaseName);
                SqlServiceBroker.New(databaseName);
                return;
            }

            if (SqlServiceBroker.IsEnabled(databaseName)) return;
            
            Logger.Trace("Enabling Service Broker for galaxy database '{Database}'", databaseName);
            SqlServiceBroker.Enable(databaseName);
        }
        
        private void InitializeDependency(string databaseName)
        {
            var connectionString = DbStringBuilder.BuildGalaxy(databaseName);
            
            Logger.Trace("Initializing SqlTableDependency for galaxy database '{Database}'", databaseName);
            _changeLogDependency = new SqlTableDependency<ChangeLog>(connectionString, ChangeLogTableName,
                mapper: DataMapper.GetChangeLogMapper(), executeUserPermissionCheck: false);
            _changeLogDependency.OnChanged += OnChangeLogTableChanged;
            _changeLogDependency.OnError += OnChangeLogTableError;
            _changeLogDependency.OnStatusChanged += OnDependencyStatusChanges;
            _changeLogDependency.Start();
        }

        private void OnChangeLogTableChanged(object sender, RecordChangedEventArgs<ChangeLog> e)
        {
            if (e.ChangeType != ChangeType.Insert) return;
            
            Logger.Debug("Change Log Table Insert Event Detected. Details: {@ChangeLog}", e.Entity);
            var changeLog = e.Entity;
            
            _archiveQueue.Enqueue(changeLog);
        }
        
        private void OnChangeLogTableError(object sender, ErrorEventArgs e)
        {
            Logger.Error(e.Error);
            Logger.Error(e.Message);
            _changeLogDependency.Stop();
            _changeLogDependency.Start();
        }
        
        private static void OnDependencyStatusChanges(object sender, StatusChangedEventArgs e)
        {
            Logger.Debug("Depepndency status changed to {Status} for galaxy {Galaxy}", e.Status, e.Database);
        }

        public void Dispose()
        {
            Logger.Trace("Stopping SqlTableDependency service");
            _changeLogDependency?.Stop();
        }
    }
}