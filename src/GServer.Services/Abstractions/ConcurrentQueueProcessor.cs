using System;
using System.Collections.Concurrent;
using System.Threading;

namespace GServer.Services.Abstractions
{
    public abstract class ConcurrentQueueProcessor<T> : IConcurrentQueueProcessor<T>
    {
        private readonly BlockingCollection<T> _queue = new();

        protected ConcurrentQueueProcessor()
        {
            var thread = new Thread(ProcessQueue) {IsBackground = true};
            thread.Start();
        }

        public virtual void Enqueue(T item)
        {
            _queue.Add(item);
        }
        
        public abstract void Process(T item);
        
        public virtual void OnComplete(T item)
        {
        }
        
        public virtual void OnError(T item, Exception e)
        {
            throw e;
        }
        
        private void ProcessQueue()
        {
            foreach (var item in _queue.GetConsumingEnumerable(CancellationToken.None))
            {
                if (item == null) continue;

                try
                {
                    Process(item);
                    OnComplete(item);
                }
                catch (Exception e)
                {
                    OnError(item, e);
                }
            }
        }
    }
}