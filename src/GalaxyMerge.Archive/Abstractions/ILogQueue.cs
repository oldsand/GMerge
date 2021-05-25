using System;
using System.Collections.Generic;
using GalaxyMerge.Archive.Entities;

namespace GalaxyMerge.Archive.Abstractions
{
    public interface ILogQueue : IDisposable
    {
        IEnumerable<QueuedLog> GetAll();
        void Enqueue(QueuedLog queuedLog);
        void Dequeue(int changeLogId);
        void MarkAsProcessing(int changeLogId);
    }
}