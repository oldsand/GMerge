using System;
using GServer.Archestra.Abstractions;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;
using GCommon.Archiving.Repositories;
using GCommon.Core.Utilities;
using GServer.Services.Abstractions;
using NLog;

namespace GServer.Services.Processors
{
    public class QueuedEntryProcessor : ConcurrentQueueProcessor<QueuedEntry>
    {
        private readonly IGalaxyRepository _galaxyRepository;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;
        private readonly IArchiveRepositoryFactory _archiveRepositoryFactory;

        public QueuedEntryProcessor(IGalaxyRepository galaxyRepository)
        {
            _galaxyRepository = galaxyRepository;
            _connectionString = DbStringBuilder.ArchiveString(galaxyRepository.Name);
        }

        public QueuedEntryProcessor(IGalaxyRepository galaxyRepository,
            IArchiveRepositoryFactory archiveRepositoryFactory)
        {
            _galaxyRepository = galaxyRepository;
            _archiveRepositoryFactory = archiveRepositoryFactory;
            _connectionString = string.Empty;
        }

        public override void Process(QueuedEntry item)
        {
            Logger.Trace("Processing queued entry with id {ChangeLogId}", item.ChangeLogId);

            using var archiveRepository = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_connectionString)
                : _archiveRepositoryFactory.Create(_connectionString);

            archiveRepository.Queue.SetProcessing(item.ChangeLogId);

            var archiveObject = archiveRepository.Objects.Get(item.ObjectId);

            using var archiver = new GalaxyArchiver(_galaxyRepository, archiveRepository);
            archiver.Archive(archiveObject);
        }

        public override void OnComplete(QueuedEntry item)
        {
            Logger.Trace("Archiving complete for entry {ChangeLogId} on object {ObjectId}", item.ChangeLogId,
                item.ObjectId);

            using var archiveRepository = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_connectionString)
                : _archiveRepositoryFactory.Create(_connectionString);

            archiveRepository.Queue.Remove(item.ChangeLogId);
        }

        public override void OnError(QueuedEntry item, Exception e)
        {
            Logger.Error(e, "Archiving failed for entry {ChangeLogId} on object {ObjectId}", item.ChangeLogId,
                item.ObjectId);

            using var archiveRepository = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_connectionString)
                : _archiveRepositoryFactory.Create(_connectionString);

            archiveRepository.Queue.SetFailed(item.ChangeLogId);
        }
    }
}