using System;
using System.Runtime.CompilerServices;
using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Archiving.Repositories;
using GalaxyMerge.Core;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Primitives;
using GalaxyMerge.Services.Base;
using NLog;

[assembly:InternalsVisibleTo("GalaxyMerge.Services.Tests")]
namespace GalaxyMerge.Services.Processors
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

        internal ChangeLogProcessor(IGalaxyDataProviderFactory dataProviderFactory,
            IArchiveRepositoryFactory archiveRepositoryFactory)
        {
            _dataProviderFactory = dataProviderFactory;
            _archiveRepositoryFactory = archiveRepositoryFactory;
        }

        public event EventHandler<QueuedEntry> OnEntryQueued;

        protected override void Process(ChangeLog item)
        {
            Logger.Trace("Processing change log with id {ChangeLogId}", item.ChangeLogId);
            
            Logger.Trace("Initializing galaxy data provider");
            using var provider = _dataProviderFactory == null 
                ? new GalaxyDataProvider(_galaxyConnectionString)
                : _dataProviderFactory.Create(_galaxyConnectionString);
            
            Logger.Trace("Initializing archive repository");
            using var repository = _archiveRepositoryFactory == null 
                ? new ArchiveRepository(_archiveConnectionString) 
                : _archiveRepositoryFactory.Create(_archiveConnectionString);
            
            Logger.Trace("Retrieving object with id {ObjectId}", item.ObjectId);
            var target = provider.Objects.Find(item.ObjectId);
            if (target == null)
            {
                Logger.Warn("Could not find object target with id {ObjectId}", item.ObjectId);
                Logger.Trace("Aborting processing for change log with id {ChangeLogId}", item.ChangeLogId);
                return;
            }
            
            Logger.Trace("Determining operation");
            var operation = Enumeration.FromId<Operation>(item.OperationId);
            if (operation == null)
            {
                Logger.Warn("Operation with id {OperationId} not valid", item.OperationId);
                Logger.Trace("Aborting processing for change log with id {ChangeLogId}", item.ChangeLogId);
                return;
            }

            Logger.Trace("Determining template");
            var template = Enumeration.FromId<Template>(target.TemplateId);
            if (template == null)
            {
                Logger.Warn("Template with id {TemplateId} not valid", item.ChangeLogId, target.TemplateId);
                Logger.Trace("Aborting processing for change log with id {ChangeLogId}", item.ChangeLogId);
                return;
            }
            
            Logger.Trace("Initializing new archive object for object id {ObjectId}", item.ObjectId);
            var archiveObject = new ArchiveObject(target.ObjectId, target.TagName, target.ConfigVersion, template);
            
            Logger.Trace("Processing object against archive settings");
            var canArchive = repository.CanArchive(archiveObject, operation);
            if (!canArchive)
            {
                Logger.Debug("ChangeLog {ChangeLogId} for Object {ObjectId} not valid for archiving",
                    item.ChangeLogId, item.ObjectId);
                Logger.Trace("Aborting processing for change log with id {ChangeLogId}", item.ChangeLogId);
                return;
            }
            
            Logger.Trace("Object {ObjectId} valid for archiving. Updating archive database", item.ObjectId);
            repository.Objects.Upsert(archiveObject);

            Logger.Trace("Creating new queued entry for change log with id {ChangeLogId}", item.ChangeLogId);
            var entry = new QueuedEntry(item.ChangeLogId, item.ObjectId, item.OperationId, item.ChangeDate);
            repository.Queue.Add(entry);
            
            Logger.Trace("Saving chagnes to archive database");
            repository.Save();
            
            OnEntryQueued?.Invoke(this, entry);
        }
    }
}