using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GClient.Data.Entities;
using NLog;

namespace GClient.Data.Abstractions
{
    public interface ILogRepository : IDisposable
    {
        LogEntry Get(int id);
        IEnumerable<LogEntry> Find(Expression<Func<LogEntry, bool>> predicate);
        IEnumerable<LogEntry> FindLatestCount(int count);
        IEnumerable<LogEntry> FindByLevel(LogLevel level);
        IEnumerable<LogEntry> FindAboveLevel(LogLevel level);
        Task<IEnumerable<LogEntry>> GetEventLogsAsync();
        IEnumerable<LogEntry> FindBelowLevel(LogLevel level);
    }
}