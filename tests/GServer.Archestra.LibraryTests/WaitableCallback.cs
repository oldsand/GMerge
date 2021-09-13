using System;
using System.Threading;

namespace GServer.Archestra.IntegrationTests
{
    internal class WaitableCallback : IDisposable
    {
        private EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        protected EventWaitHandle WaitHandle => this.waitHandle;

        public void WaitOnOperation() => this.waitHandle.WaitOne();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this.waitHandle == null)
                return;
            this.waitHandle.Dispose();
            this.waitHandle = (EventWaitHandle)null;
        }
    }
}