using System;

namespace GServer.Services.Abstractions
{
    public interface IConcurrentQueueProcessor<in T>
    {
        void Enqueue(T item);
        void Process(T item);
        void OnComplete(T item);
        void OnError(T item, Exception e);
    }
}