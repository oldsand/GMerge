using System;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;
using GCommon.Archiving.Repositories;
using GCommon.Core.Utilities;
using GCommon.Data;
using GCommon.Data.Abstractions;
using GCommon.Data.Entities;
using GServer.Services.Abstractions;
using NLog;

namespace GServer.Services.Processors
{
    public class ChangeLogProcessor : ConcurrentQueueProcessor<ChangeLog>
    {
        private readonly IGalaxyDataProviderFactory _dataProviderFactory;
        private readonly IArchiveRepositoryFactory _archiveRepositoryFactory;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _archiveConnectionString;
        private readonly string _galaxyConnectionString;

        public ChangeLogProcessor(string galaxyName)
        {
            _archiveConnectionString = DbStringBuilder.ArchiveString(galaxyName);
            _galaxyConnectionString = DbStringBuilder.GalaxyString(galaxyName);
        }

        public ChangeLogProcessor(IGalaxyDataProviderFactory dataProviderFactory,
            IArchiveRepositoryFactory archiveRepositoryFactory)
        {
            _dataProviderFactory = dataProviderFactory;
            _archiveRepositoryFactory = archiveRepositoryFactory;
            _archiveConnectionString = string.Empty;
            _galaxyConnectionString = string.Empty;
        }

        public event EventHandler<QueuedEntry> OnEntryQueued;

        public override void Process(ChangeLog item)
        {
            Logger.Trace("Processing change log with id {ChangeLogId}", item.ChangeLogId);
            
            using var provider = _dataProviderFactory == null 
                ? new GalaxyDataProvider(_galaxyConnectionString)
                : _dataProviderFactory.Create(_galaxyConnectionString);
            
            using var repository = _archiveRepositoryFactory == null 
                ? new ArchiveRepository(_archiveConnectionString) 
                : _archiveRepositoryFactory.Create(_archiveConnectionString);
            
            var target = provider.Objects.Find(item.ObjectId);
            if (target == null)
            {
                Logger.Warn("Could not find object target with id {ObjectId}", item.ObjectId);
                Logger.Trace("Aborting processing for change log with id {ChangeLogId}", item.ChangeLogId);
                return;
            }
            
            var archiveObject = new ArchiveObject(target.ObjectId, target.TagName, target.ConfigVersion, target.Template);
            var canArchive = repository.CanArchive(archiveObject, item.Operation);
            if (!canArchive)
            {
                Logger.Debug("ChangeLog {ChangeLogId} for Object {ObjectId} not valid for archiving",
                    item.ChangeLogId, item.ObjectId);
                Logger.Trace("Aborting processing for change log with id {ChangeLogId}", item.ChangeLogId);
                return;
            }
            
            Logger.Debug("Object {ObjectId} valid for archiving. Updating archive database", item.ObjectId);
            repository.Objects.Upsert(archiveObject);
            
            var entry = new QueuedEntry(item.ChangeLogId, item.ObjectId, item.OperationId, item.ChangeDate);
            repository.Queue.Add(entry);
            
            repository.Save();
            
            OnEntryQueued?.Invoke(this, entry);
        }

        public override void OnError(ChangeLog item, Exception e)
        {
            
        }
    }
}