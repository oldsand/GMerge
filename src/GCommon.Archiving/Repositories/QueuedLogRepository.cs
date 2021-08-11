using GCommon.Archiving.Abstractions;
using GCommon.Primitives;

namespace GCommon.Archiving.Repositories
{
    internal class QueuedLogRepository : IQueuedLogRepository
    {
        private readonly ArchiveContext _context;

        public QueuedLogRepository(ArchiveContext context)
        {
            _context = context;
        }
        
        public void Dispose()
        {
            _context?.Dispose();
        }

        public QueuedLog Get(int changelogId)
        {
            return _context.Queue.Find(changelogId);
        }

        public void Enqueue(int changeLogId)
        {
            var target = _context.Queue.Find(changeLogId);
            if (target != null) return;
            
            var log = new QueuedLog(changeLogId);
            _context.Queue.Add(log);
        }

        public void Dequeue(int changeLogId)
        {
            var target = _context.Queue.Find(changeLogId);
            if (target == null) return;
            
            _context.Queue.Remove(target);
        }
    }
}