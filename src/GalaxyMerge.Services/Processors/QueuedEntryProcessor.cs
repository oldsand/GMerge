using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Services.Base;

namespace GalaxyMerge.Services.Processors
{
    public class QueuedEntryProcessor : ConcurrentQueueProcessor<QueuedEntry>
    {
        private readonly string _connectionString;

        public QueuedEntryProcessor(string galaxyName)
        {
            _connectionString = DbStringBuilder.ArchiveString(galaxyName);
        }

        protected override void Process(QueuedEntry item)
        {
            throw new System.NotImplementedException();
        }
    }
}