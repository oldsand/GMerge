using System;
using GServer.Archestra.Abstractions;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;
using GCommon.Archiving.Repositories;
using GCommon.Core.Utilities;
using GCommon.Primitives;
using GServer.Services.Abstractions;
using NLog;

namespace GServer.Services.Processors
{
    public class ArchiveLogProcessor : ConcurrentQueueProcessor<ArchiveLog>
    {
        private readonly IGalaxyRepository _galaxyRepository;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;
        private readonly IArchiveRepositoryFactory _archiveRepositoryFactory;

        public ArchiveLogProcessor(IGalaxyRepository galaxyRepository)
        {
            _galaxyRepository = galaxyRepository;
            _connectionString = DbStringBuilder.ArchiveString(galaxyRepository.Name);
        }

        public ArchiveLogProcessor(IGalaxyRepository galaxyRepository,
            IArchiveRepositoryFactory archiveRepositoryFactory)
        {
            _galaxyRepository = galaxyRepository;
            _archiveRepositoryFactory = archiveRepositoryFactory;
            _connectionString = string.Empty;
        }

        protected override void Process(ArchiveLog item)
        {
            Logger.Trace("Processing archive log with id {ChangeLogId}", item.ChangeLogId);

            using var archiveRepository = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_connectionString)
                : _archiveRepositoryFactory.Create(_connectionString);
            
            Logger.Trace("Updating archive log state to processing");
            item.State = ArchiveState.Processing;
            archiveRepository.Logs.Update(item);
            archiveRepository.Save();

            //var archiveObject = archiveRepository.Objects.Get(item.ObjectId);
            var archiveObject = item.ArchiveObject;

            using var archiver = new GalaxyArchiver(_galaxyRepository, archiveRepository);
            archiver.Archive(archiveObject);
            
            var entry = archiveObject.GetLatestEntry();
            entry.AssignLog(item);
            archiveRepository.Save();
        }

        protected override void OnComplete(ArchiveLog item)
        {
            Logger.Trace("Archiving complete for entry {ChangeLogId} on object {ObjectId}", item.ChangeLogId,
                item.ObjectId);

            using var archiveRepository = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_connectionString)
                : _archiveRepositoryFactory.Create(_connectionString);

            item.State = ArchiveState.Archived;
            archiveRepository.Logs.Update(item);
            archiveRepository.Save();
        }

        protected override void OnError(ArchiveLog item, Exception e)
        {
            Logger.Error(e, "Archiving failed for entry {ChangeLogId} on object {ObjectId}", item.ChangeLogId,
                item.ObjectId);

            using var archiveRepository = _archiveRepositoryFactory == null
                ? new ArchiveRepository(_connectionString)
                : _archiveRepositoryFactory.Create(_connectionString);

            item.State = ArchiveState.Failed;
            archiveRepository.Logs.Update(item);
            archiveRepository.Save();
        }
    }
}