using System;
using System.Collections.Concurrent;
using System.Threading;

namespace GalaxyMerge.Services.Base
{
    public interface IConcurrentQueueProcessor<T>
    {
        void Enqueue(T item);
        event EventHandler<T> OnComplete;
        event EventHandler<Exception> OnError;
    }

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

        public event EventHandler<T> OnComplete;
        public event EventHandler<Exception> OnError;

        protected abstract void Process(T item);

        private void ProcessQueue()
        {
            foreach (var item in _queue.GetConsumingEnumerable(CancellationToken.None))
            {
                if (item == null) continue;

                try
                {
                    Process(item);
                    OnComplete?.Invoke(this, item);
                }
                catch (Exception e)
                {
                    OnError?.Invoke(this, e);
                }
            }
        }
    }
}