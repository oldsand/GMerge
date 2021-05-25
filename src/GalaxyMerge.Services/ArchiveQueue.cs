using System;
using System.Collections.Concurrent;
using System.Threading;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Repositories;
using NLog;

namespace GalaxyMerge.Services
{
    public class ArchiveQueue
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IGalaxyRepository _galaxyRepository;
        private readonly BlockingCollection<QueuedLog> _jobQueue = new BlockingCollection<QueuedLog>();

        public ArchiveQueue(IGalaxyRepository galaxyRepository)
        {
            _galaxyRepository = galaxyRepository;
            var thread = new Thread(ProcessJobs) {IsBackground = true};
            thread.Start();
        }
 
        public void Enqueue(ChangeLog changeLog)
        {
            Logger.Info("Enqueuing change log '{ChangeLogId}' for object '{ObjectId}'", changeLog.ChangeLogId,
                changeLog.ObjectId);
            
            var queuedLog = new QueuedLog(changeLog.ChangeLogId, changeLog.ObjectId);
            
            using var logQueue = new LogQueue(_galaxyRepository.Name);
            logQueue.Enqueue(queuedLog);
            
            _jobQueue.Add(queuedLog);
        }

        private void ProcessJobs()
        {
            ReloadQueue();
            
            foreach (var log in _jobQueue.GetConsumingEnumerable(CancellationToken.None))
            {
                using var logQueue = new LogQueue(_galaxyRepository.Name);
                
                Console.WriteLine("Validating against queue settings:" + log.ChangeLogId);
                if (!CanQueue(log))
                {
                    Console.WriteLine("Invalid archive operation:" + log.ChangeLogId);
                    logQueue.Dequeue(log.ChangeLogId);
                    continue;
                }

                Console.WriteLine("Valid archive operation - Marking queued log processing:" + log.ChangeLogId);
                logQueue.MarkAsProcessing(log.ChangeLogId);
                
                Console.WriteLine("Processing object:" + log.ObjectId);
                var archiver = new ArchiveProcessor(_galaxyRepository);
                archiver.Archive(log.ObjectId, log.ChangeLogId);
                
                Console.WriteLine("Dequeuing log:" + log.ChangeLogId);
                logQueue.Dequeue(log.ChangeLogId);
                
                Console.WriteLine("Processing complete for log:" + log.ChangeLogId);
            }
        }

        private void ReloadQueue()
        {
            Logger.Info("Loading queued logs from database.");
            
            using var logQueue = new LogQueue(_galaxyRepository.Name);
            var queuedLogs = logQueue.GetAll();

            foreach (var queuedLog in queuedLogs)
                _jobQueue.Add(queuedLog);
        }

        private bool CanQueue(QueuedLog queuedLog)
        {
            using var repo = new ChangeLogRepository(_galaxyRepository.Name);
            var changeLog = repo.Find(x => x.ChangeLogId == queuedLog.ChangeLogId);
            
            var helper = new ArchiveHelper(_galaxyRepository.Name);
            return helper.IsArchivable(changeLog);
        }
    }
}