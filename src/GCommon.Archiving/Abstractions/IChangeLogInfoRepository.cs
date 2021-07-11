using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GCommon.Archiving.Entities;

namespace GCommon.Archiving.Abstractions
{
    public interface IChangeLogInfoRepository : IDisposable
    {
        ChangeLogInfo Get(int changeLogId);
        IEnumerable<ChangeLogInfo> GetAll();
        IEnumerable<ChangeLogInfo> Find(Expression<Func<ChangeLogInfo, bool>> predicate);
        void Add(ChangeLogInfo changeLog);
    }
}