using System;
using GalaxyMerge.Archiving.Entities;

namespace GalaxyMerge.Services.Abstractions
{
    public interface IArchiveQueue
    {
        void Enqueue(QueuedEntry entry, Action<QueuedEntry> processor);
    }
}