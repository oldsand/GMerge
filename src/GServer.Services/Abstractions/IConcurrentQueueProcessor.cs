using System;

namespace GServer.Services.Abstractions
{
    public interface IConcurrentQueueProcessor<in T>
    {
        void Enqueue(T item);
    }
}