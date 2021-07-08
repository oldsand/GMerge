using System;
using System.Collections.Concurrent;
using System.Threading;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Services.Abstractions;
using NLog;

namespace GalaxyMerge.Services
{
    public class ArchiveQueue : IArchiveQueue
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly BlockingCollection<Tuple<QueuedEntry, Action<QueuedEntry>>> _processQueue = new();

        public ArchiveQueue()
        {
            Logger.Trace("Initializing new archive archive queue");
            var thread = new Thread(Process) {IsBackground = true};
            thread.Start();
        }

        public void Enqueue(QueuedEntry entry, Action<QueuedEntry> processor)
        {
            Logger.Trace("Enqueuing entry with change log id '{ChangeLogId}'", entry.ChangeLogId);
            _processQueue.Add(new Tuple<QueuedEntry, Action<QueuedEntry>>(entry, processor));
        }
        
        private void Process()
        {
            foreach (var (entry, processor) in _processQueue.GetConsumingEnumerable(CancellationToken.None))
            {
                Logger.Trace("Processing entry with change log id '{ChangeLogId}'", entry.ChangeLogId);
                processor?.Invoke(entry);
            }
        }
    }
}