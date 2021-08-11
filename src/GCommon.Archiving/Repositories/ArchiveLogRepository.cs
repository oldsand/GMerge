using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GCommon.Archiving.Abstractions;
using GCommon.Primitives;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Archiving.Repositories
{
    internal class ArchiveLogRepository : IArchiveLogRepository
    {
        private readonly ArchiveContext _context;

        public ArchiveLogRepository(ArchiveContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public ArchiveLog Get(int changeLogId)
        {
            return _context.Logs
                .Include(x => x.ArchiveObject)
                .Include(x => x.EntryLog)
                .SingleOrDefault(x => x.ChangeLogId == changeLogId);
        }

        public IEnumerable<ArchiveLog> GetAll()
        {
            return _context.Logs;
        }

        public IEnumerable<ArchiveLog> Find(Expression<Func<ArchiveLog, bool>> predicate)
        {
            return _context.Logs.Where(predicate);
        }

        public void Add(ArchiveLog log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log), "log can not be null");
            _context.Logs.Add(log);
        }

        public void Update(ArchiveLog log)
        {
            if (log == null) throw new ArgumentNullException(nameof(log), "log can not be null");
            _context.Logs.Update(log);
        }
    }
}