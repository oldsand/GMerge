using System;
using System.Collections.Generic;
using GalaxyMerge.Archive.Entities;

namespace GalaxyMerge.Archive.Abstractions
{
    public interface IQueueRepository : IDisposable
    {
        IEnumerable<QueuedEntry> GetAll();
        void Add(QueuedEntry queuedEntry);
        void Remove(int changeLogId);
        void SetProcessing(int changeLogId);
        void SetFailed(int changeLogId);
    }
}