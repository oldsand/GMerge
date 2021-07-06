using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GalaxyMerge.Archiving.Entities;

namespace GalaxyMerge.Archiving.Abstractions
{
    public interface IQueueRepository : IDisposable
    {
        QueuedEntry Get(int changeLogId);
        IEnumerable<QueuedEntry> GetAll();
        IEnumerable<QueuedEntry> Find(Expression<Func<QueuedEntry, bool>> predicate);
        void Add(QueuedEntry queuedEntry);
        void Remove(int changeLogId);
        void SetProcessing(int changeLogId);
        void SetFailed(int changeLogId);
    }
}