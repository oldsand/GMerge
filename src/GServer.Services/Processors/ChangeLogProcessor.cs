using System;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;
using GCommon.Archiving.Repositories;
using GCommon.Core.Utilities;
using GCommon.Data;
using GCommon.Data.Abstractions;
using GCommon.Data.Entities;
using GServer.Archestra.Abstractions;
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
        private readonly ArchiveLogProcessor _archiveLogProcessor;

        public ChangeLogProcessor(IGalaxyRepository galaxyRepository)
        {
            _archiveLogProcessor = new ArchiveLogProcessor(galaxyRepository);
            _archiveConnectionString = DbStringBuilder.ArchiveString(galaxyRepository.Name);
            _galaxyConnectionString = DbStringBuilder.GalaxyString(galaxyRepository.Name);
        }

        public ChangeLogProcessor(IGalaxyDataProviderFactory dataProviderFactory,
            IArchiveRepositoryFactory archiveRepositoryFactory)
        {
            _dataProviderFactory = dataProviderFactory;
            _archiveRepositoryFactory = archiveRepositoryFactory;
            _archiveConnectionString = string.Empty;
            _galaxyConnectionString = string.Empty;
        }

        protected override void Process(ChangeLog item)
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

            var archiveObject =
                new ArchiveObject(target.ObjectId, target.TagName, target.ConfigVersion, target.Template);

            if (!repository.CanArchive(archiveObject, item.Operation))
            {
                Logger.Debug("ChangeLog {ChangeLogId} for Object {ObjectId} not valid for archiving",
                    item.ChangeLogId, item.ObjectId);
                Logger.Trace("Aborting processing for change log with id {ChangeLogId}", item.ChangeLogId);
                return;
            }

            Logger.Debug("ChangeLog {ChangeLogId} for Object {ObjectId} valid for archiving", item.ChangeLogId,
                item.ObjectId);

            archiveObject.AddLog(item.ChangeLogId, item.ChangeDate, item.Operation, item.Comment, item.UserName);
            repository.Objects.Upsert(archiveObject);
            repository.Save();
        }

        protected override void OnComplete(ChangeLog item)
        {
            using var repository = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_archiveConnectionString)
                : _archiveRepositoryFactory.Create(_archiveConnectionString);

            var log = repository.Logs.Get(item.ChangeLogId);

            if (log == null) return;
            
            _archiveLogProcessor.Enqueue(log);
        }

        protected override void OnError(ChangeLog item, Exception e)
        {
            //todo probably want to fire events 
        }
    }
}