using System;
using System.Collections.Concurrent;
using System.Threading;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Data.Entities;
using NLog;

namespace GalaxyMerge.Services
{
    public class ArchiveQueue
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly BlockingCollection<QueuedEntry> _jobQueue = new BlockingCollection<QueuedEntry>();
        private readonly IGalaxyRepository _galaxyRepository;
        private readonly ArchiveProcessor _archiveProcessor;

        public ArchiveQueue(IGalaxyRepository galaxyRepository)
        {
            _galaxyRepository = galaxyRepository;
            _archiveProcessor = new ArchiveProcessor(galaxyRepository);
            var thread = new Thread(ProcessJobs) {IsBackground = true};
            thread.Start();
        }
 
        public void Enqueue(ChangeLog changeLog)
        {
            Logger.Info("Enqueuing change log '{ChangeLogId}' for object '{ObjectId}'", changeLog.ChangeLogId,
                changeLog.ObjectId);
            
            var queuedEntry = new QueuedEntry(changeLog.ChangeLogId, changeLog.ObjectId, changeLog.OperationId);
            
            using var queueRepository = new QueueRepository(_galaxyRepository.Name);
            queueRepository.Add(queuedEntry);
            
            _jobQueue.Add(queuedEntry);
        }

        private void ProcessJobs()
        {
            ReloadQueue();
            
            foreach (var entry in _jobQueue.GetConsumingEnumerable(CancellationToken.None))
            {
                Logger.Trace("New entry with id '{ChangeLogId}' detected", entry.ChangeLogId);
                using var queueRepository = new QueueRepository(_galaxyRepository.Name);
                
                if (!CanProcess(entry))
                {
                    Logger.Trace("Operation '{OperationId}' for object '{ObjectId}' not valid for processing"
                        ,entry.OperationId, entry.ObjectId);
                    queueRepository.Remove(entry.ChangeLogId);
                    continue;
                }

                try
                {
                    Logger.Info("Archiving entry '{ChangeLogId}' for object '{ObjectId}'"
                        ,entry.ChangeLogId ,entry.ObjectId);
                    queueRepository.SetProcessing(entry.ChangeLogId);
                    _archiveProcessor.Archive(entry.ObjectId, entry.ChangeLogId);
                    queueRepository.Remove(entry.ChangeLogId);
                }
                catch (Exception)
                {
                    Logger.Error("Archiving failed for '{ChangeLogId}' on object '{ObjectId}'"
                        ,entry.ChangeLogId ,entry.ObjectId);
                    queueRepository.SetFailed(entry.ChangeLogId);
                }
            }
        }

        private void ReloadQueue()
        {
            Logger.Info("Loading queued logs from database");
            
            using var logQueue = new QueueRepository(_galaxyRepository.Name);
            var queuedLogs = logQueue.GetAll();

            foreach (var queuedLog in queuedLogs)
                _jobQueue.Add(queuedLog);
        }

        private bool CanProcess(QueuedEntry queuedEntry)
        {
            var helper = new ArchiveHelper(_galaxyRepository.Name);
            return helper.IsArchivable(queuedEntry);
        }
    }
}