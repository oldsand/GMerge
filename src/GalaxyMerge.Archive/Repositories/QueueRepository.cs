using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GalaxyMerge.Archive.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive.Repositories
{
    public class QueueRepository : IQueueRepository
    {
        private readonly ArchiveContext _context;

        public QueueRepository(string galaxyName)
        {
            var connectionString = DbStringBuilder.BuildArchive(galaxyName);
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            _context = new ArchiveContext(options);
        }

        public QueuedEntry Get(int changeLogId)
        {
            return _context.QueuedEntries.Find(changeLogId);
        }

        public IEnumerable<QueuedEntry> GetAll()
        {
            return _context.QueuedEntries.ToList();
        }

        public IEnumerable<QueuedEntry> Find(Expression<Func<QueuedEntry,bool>> predicate)
        {
            return _context.QueuedEntries.Where(predicate).ToList();
        }

        public void Add(QueuedEntry queuedEntry)
        {
            _context.QueuedEntries.Add(queuedEntry);
            _context.SaveChanges();
        }

        public void Remove(int changeLogId)
        {
            var item = _context.QueuedEntries.Find(changeLogId);
            _context.QueuedEntries.Remove(item);
            _context.SaveChanges();
        }

        public void SetProcessing(int changeLogId)
        {
            var item = _context.QueuedEntries.Find(changeLogId);
            item.State = QueueState.Processing;
            _context.SaveChanges();
        }

        public void SetFailed(int changeLogId)
        {
            var item = _context.QueuedEntries.Find(changeLogId);
            item.State = QueueState.Failed;
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}