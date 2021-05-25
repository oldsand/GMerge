using System;
using System.Collections.Concurrent;
using System.Threading;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Services
{
    public class ArchiveQueue
    {
        private readonly IGalaxyRepository _galaxyRepository;
        private readonly BlockingCollection<QueuedLog> _jobQueue = new BlockingCollection<QueuedLog>();

        public ArchiveQueue(IGalaxyRepository galaxyRepository)
        {
            _galaxyRepository = galaxyRepository;
            var thread = new Thread(OnStart) {IsBackground = true};
            thread.Start();
        }
 
        public void Enqueue(ChangeLog changeLog)
        {
            if (!CanQueue(changeLog)) return;

            var log = new QueuedLog(changeLog.ChangeLogId, changeLog.ObjectId);
            
            Console.WriteLine("Persisting log to archive queue:" + log.ChangeLogId);
            using var logQueue = new LogQueue(_galaxyRepository.Name);
            logQueue.Enqueue(log);
            
            Console.WriteLine("Adding Log to job queue:" + log.ChangeLogId);
            _jobQueue.Add(log);
        }

        private void OnStart()
        {
            RefreshQueue();
            
            foreach (var log in _jobQueue.GetConsumingEnumerable(CancellationToken.None))
            {
                Console.WriteLine("Marking queued log processing:" + log.ChangeLogId);
                using var queue = new LogQueue(_galaxyRepository.Name);
                queue.MarkAsProcessing(log.ChangeLogId);
                
                Console.WriteLine("Processing object:" + log.ObjectId);
                var archiver = new ArchiveProcessor(_galaxyRepository);
                archiver.Archive(log.ObjectId, log.ChangeLogId);
                
                Console.WriteLine("Dequeuing log:" + log.ChangeLogId);
                queue.Dequeue(log.ChangeLogId);
            }
        }

        private void RefreshQueue()
        {
            using var logQueue = new LogQueue(_galaxyRepository.Name);
            var queuedLogs = logQueue.GetAll();

            foreach (var queuedLog in queuedLogs)
                _jobQueue.Add(queuedLog);
        }

        private bool CanQueue(ChangeLog changeLog)
        {
            var helper = new ArchiveHelper(_galaxyRepository.Name);
            return helper.IsArchivable(changeLog);
        }
    }
}