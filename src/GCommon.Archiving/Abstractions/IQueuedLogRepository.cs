using System;
using GCommon.Archiving.Entities;

namespace GCommon.Archiving.Abstractions
{
    public interface IQueuedLogRepository : IDisposable
    {
        QueuedLog Get(int changelogId);
        void Enqueue(int changeLogId);
        void Dequeue(int changeLogId);
    }
}