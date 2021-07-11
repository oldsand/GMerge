using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;

namespace GCommon.Archiving.Repositories
{
    internal class ChangeLogInfoRepository : IChangeLogInfoRepository
    {
        private readonly ArchiveContext _context;

        public ChangeLogInfoRepository(ArchiveContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public ChangeLogInfo Get(int changeLogId)
        {
            return _context.ChangeLogs.Find(changeLogId);
        }

        public IEnumerable<ChangeLogInfo> GetAll()
        {
            return _context.ChangeLogs;
        }

        public IEnumerable<ChangeLogInfo> Find(Expression<Func<ChangeLogInfo, bool>> predicate)
        {
            return _context.ChangeLogs.Where(predicate);
        }

        public void Add(ChangeLogInfo changeLog)
        {
            _context.ChangeLogs.Add(changeLog);
        }
    }
}