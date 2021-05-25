using System;
using System.Collections.Concurrent;
using System.Threading;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Services
{
    public class ArchiveQueue
    {
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
            var queuedLog = new QueuedLog(changeLog.ChangeLogId, changeLog.ObjectId);
            
            Console.WriteLine("Saving log to queue:" + queuedLog.ChangeLogId);
            using var logQueue = new LogQueue(_galaxyRepository.Name);
            logQueue.Enqueue(queuedLog);
            
            Console.WriteLine("Adding log to job queue:" + queuedLog.ChangeLogId);
            _jobQueue.Add(queuedLog);
        }

        private void ProcessJobs()
        {
            RefreshQueue();
            
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

        private void RefreshQueue()
        {
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