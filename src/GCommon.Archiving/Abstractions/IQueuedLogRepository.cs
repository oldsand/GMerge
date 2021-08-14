using System;
using GCommon.Primitives;

namespace GCommon.Archiving.Abstractions
{
    public interface IQueuedLogRepository : IDisposable
    {
        QueuedLog Get(int changelogId);
        void Enqueue(QueuedLog log);
        void Dequeue(int changeLogId);
    }
}