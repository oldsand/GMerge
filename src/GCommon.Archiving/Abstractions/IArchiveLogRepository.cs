using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GCommon.Archiving.Entities;

namespace GCommon.Archiving.Abstractions
{
    public interface IArchiveLogRepository : IDisposable
    {
        ArchiveLog Get(int changeLogId);
        IEnumerable<ArchiveLog> GetAll();
        IEnumerable<ArchiveLog> Find(Expression<Func<ArchiveLog, bool>> predicate);
        void Update(ArchiveLog log);
    }
}