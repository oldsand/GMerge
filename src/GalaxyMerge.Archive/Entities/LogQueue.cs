using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Archive.Abstractions;
using GalaxyMerge.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive.Entities
{
    public class LogQueue : ILogQueue
    {
        private readonly ArchiveContext _context;

        public LogQueue(string galaxyName)
        {
            var connectionString = DbStringBuilder.BuildArchive(galaxyName);
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            _context = new ArchiveContext(options);
        }

        public IEnumerable<QueuedLog> GetAll()
        {
            return _context.QueuedLogs.ToList();
        }

        public void Enqueue(QueuedLog queuedLog)
        {
            _context.QueuedLogs.Add(queuedLog);
            _context.SaveChanges();
        }

        public void Dequeue(int changeLogId)
        {
            var item = _context.QueuedLogs.Find(changeLogId);
            _context.QueuedLogs.Remove(item);
            _context.SaveChanges();
        }

        public void MarkAsProcessing(int changeLogId)
        {
            var item = _context.QueuedLogs.Find(changeLogId);
            item.IsProcessing = true;
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}