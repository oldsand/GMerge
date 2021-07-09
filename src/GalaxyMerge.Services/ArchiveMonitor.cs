using System;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Services.Abstractions;
using GalaxyMerge.Services.Processors;
using NLog;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace GalaxyMerge.Services
{
    public class ArchiveMonitor : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private SqlTableDependency<ChangeLog> _changeLogDependency;
        private readonly ChangeLogProcessor _changeLogProcessor;
        private readonly QueuedEntryProcessor _queuedEntryProcessor;
        private const string ChangeLogTableName = "gobject_change_log";

        public ArchiveMonitor(string galaxyName)
        {
            Logger.Trace("Initializing archive processors for '{Galaxy}'",galaxyName);
            _changeLogProcessor = new ChangeLogProcessor(galaxyName);
            _changeLogProcessor.OnEntryQueued += OnEntryQueued;
            _queuedEntryProcessor = new QueuedEntryProcessor(galaxyName);

            SetupServiceBroker(galaxyName);
            InitializeDependency(galaxyName);
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
            var connectionString = DbStringBuilder.GalaxyString(databaseName);

            Logger.Trace("Initializing Table Dependency for galaxy database '{Database}'", databaseName);
            _changeLogDependency = new SqlTableDependency<ChangeLog>(connectionString, ChangeLogTableName,
                mapper: DataMapper.GetChangeLogMapper(), executeUserPermissionCheck: false);
            
            _changeLogDependency.OnChanged += OnChangeLogTableChanged;
            _changeLogDependency.OnError += OnChangeLogTableError;
            _changeLogDependency.OnStatusChanged += OnDependencyStatusChanges;

            Logger.Debug("Starting ChangeLog Table Dependency for '{Database}'", databaseName);
            _changeLogDependency.Start();
        }

        private void OnChangeLogTableChanged(object sender, RecordChangedEventArgs<ChangeLog> e)
        {
            if (e.ChangeType != ChangeType.Insert) return;
            Logger.Trace("Change log event detected. Details: {@ChangeLog}", e.Entity);
            _changeLogProcessor.Enqueue(e.Entity);
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
            Logger.Trace("Depepndency status changed to {Status} for galaxy {Galaxy}", e.Status, e.Database);
            
            if (e.Status == TableDependencyStatus.Started)
                Logger.Debug("Archive monitor table dependency started on {GalaxyName}", e.Database);
        }
        
        private void OnEntryQueued(object sender, QueuedEntry e)
        {
            _queuedEntryProcessor.Enqueue(e);
        }

        public void Dispose()
        {
            Logger.Trace("Stopping table dependency service");
            _changeLogProcessor.OnEntryQueued -= OnEntryQueued;
            _changeLogDependency?.Stop();
        }
    }
}