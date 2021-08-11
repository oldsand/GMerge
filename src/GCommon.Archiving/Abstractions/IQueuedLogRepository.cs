using System;
using GCommon.Primitives;

namespace GCommon.Archiving.Abstractions
{
    public interface IQueuedLogRepository : IDisposable
    {
        QueuedLog Get(int changelogId);
        void Enqueue(int changeLogId);
        void Dequeue(int changeLogId);
    }
}