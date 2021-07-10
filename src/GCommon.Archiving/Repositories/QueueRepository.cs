using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GCommon.Primitives;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;

namespace GCommon.Archiving.Repositories
{
    internal class QueueRepository : IQueueRepository
    {
        private readonly ArchiveContext _context;

        public QueueRepository(ArchiveContext context)
        {
            _context = context;
        }

        public QueuedEntry Get(int changeLogId)
        {
            return _context.Queue.Find(changeLogId);
        }

        public IEnumerable<QueuedEntry> GetAll()
        {
            return _context.Queue.ToList();
        }

        public IEnumerable<QueuedEntry> Find(Expression<Func<QueuedEntry, bool>> predicate)
        {
            return _context.Queue.Where(predicate).ToList();
        }

        public void Add(QueuedEntry queuedEntry)
        {
            _context.Queue.Add(queuedEntry);
        }

        public void Remove(int changeLogId)
        {
            var target = _context.Queue.Find(changeLogId);
            _context.Queue.Remove(target);
        }

        public void SetProcessing(int changeLogId)
        {
            var target = _context.Queue.Find(changeLogId);
            target.State = QueueState.Processing;
        }

        public void SetFailed(int changeLogId)
        {
            var target = _context.Queue.Find(changeLogId);
            target.State = QueueState.Failed;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}