using System;
using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Services.Abstractions;
using NLog;

namespace GalaxyMerge.Services
{
    public class ArchiveProcessor : IArchiveProcessor
    {
        private readonly string _galaxyName;
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly IDataRepositoryFactory _dataRepositoryFactory;
        private readonly IArchiveRepositoryFactory _archiveRepositoryFactory;
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IArchiveQueue _archiveQueue;

        public ArchiveProcessor(string galaxyName,
            IGalaxyRegistry galaxyRegistry,
            IDataRepositoryFactory dataRepositoryFactory, 
            IArchiveRepositoryFactory archiveRepositoryFactory,
            IArchiveQueue archiveQueue)
        {
            _galaxyName = galaxyName;
            _galaxyRegistry = galaxyRegistry;
            _dataRepositoryFactory = dataRepositoryFactory;
            _archiveRepositoryFactory = archiveRepositoryFactory;
            _archiveQueue = archiveQueue;
        }

        public void Enqueue(ChangeLog changeLog)
        {
            Logger.Debug("Adding change log '{ChangeLogId}' to archive queue", changeLog.ChangeLogId);
            
            var entry = new QueuedEntry(changeLog.ChangeLogId, changeLog.ObjectId, changeLog.OperationId, changeLog.ChangeDate);
            using var archiveRepository = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyName));
            archiveRepository.Queue.Add(entry);
            
            _archiveQueue.Enqueue(entry, Process);
        }

        private void Process(QueuedEntry entry)
        {
            Logger.Trace("Initializing repositories for galaxy '{GalaxyName}'", _galaxyName);
            var galaxyRepository = _galaxyRegistry.GetByCurrentIdentity(_galaxyName);
            using var dataRepository = _dataRepositoryFactory.Create(DbStringBuilder.GalaxyString(_galaxyName));
            using var archiveRepository = _archiveRepositoryFactory.Create(DbStringBuilder.ArchiveString(_galaxyName));
            
            Logger.Debug("Starting processing for {ChangeLogId}", entry.ChangeLogId);
            archiveRepository.Queue.SetProcessing(entry.ChangeLogId);

            Logger.Trace("Retrieving gObjet {ObjectId}", entry.ObjectId);
            var target = dataRepository.Objects.Find(entry.ObjectId);
            if (target == null)
                throw new InvalidOperationException($"Could not find object target with id {entry.ObjectId}");

            Logger.Trace("Retrieving archive");
            var archive = archiveRepository.Get();

            var canArchive =
                archive.CanArchive(target.ObjectId, target.TemplateId, target.IsTemplate, entry.OperationId);

            if (!canArchive)
            {
                Logger.Debug("ChangeLog {ChangeLogId} not configured for archiving. Removing from archive queue", entry.ChangeLogId);
                archiveRepository.Queue.Remove(entry.ChangeLogId);
                return;
            }

            try
            {
                Logger.Trace("Archiving log {ChangeLogId} for object {ObjectId}", entry.ChangeLogId,
                    entry.ObjectId);
                
                using var archiver = new Archiver(galaxyRepository, dataRepository, archiveRepository);
                archiver.Archive(target, false, entry.ChangeLogId);

                Logger.Debug("Archiving complete for log {ChangeLogId} on object {ObjectId}",
                    entry.ChangeLogId, entry.ObjectId);
                
                archiveRepository.Queue.Remove(entry.ChangeLogId);
            }
            catch (Exception)
            {
                Logger.Error("Archiving failed for log {ChangeLogId} on object {ObjectId}"
                    , entry.ChangeLogId, entry.ObjectId);
                archiveRepository.Queue.SetFailed(entry.ChangeLogId);
            }
        }
    }
}