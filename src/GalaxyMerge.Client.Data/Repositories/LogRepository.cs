using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GalaxyMerge.Client.Data.Abstractions;
using GalaxyMerge.Client.Data.Entities;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace GalaxyMerge.Client.Data.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly AppContext _context;
        private readonly DbSet<LogEntry> _logs;

        public LogRepository()
        {
            _context = new AppContext();
            _logs = _context.Logs;
        }
        
        public LogRepository(string dataSource)
        {
            var options = new DbContextOptionsBuilder<AppContext>().UseSqlite(dataSource).Options;
            _context = new AppContext(options);
            _logs = _context.Logs;
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }

        public LogEntry Get(int id)
        {
            return _logs.Find(id);
        }

        public IEnumerable<LogEntry> Find(Expression<Func<LogEntry, bool>> predicate)
        {
            return _logs.Where(predicate).ToList();
        }

        public IEnumerable<LogEntry> FindLatestCount(int count)
        {
            return _logs.OrderByDescending(x => x.Logged).Take(count).ToList();
        }

        public IEnumerable<LogEntry> FindByLevel(LogLevel level)
        {
            return _logs.Where(x => x.LogLevel == level).ToList();
        }
        
        public IEnumerable<LogEntry> FindAboveLevel(LogLevel level)
        {
            return _logs.Where(x => x.LevelId > level.Ordinal).ToList();
        }
        
        public IEnumerable<LogEntry> FindBelowLevel(LogLevel level)
        {
            return _logs.Where(x => x.LevelId < level.Ordinal).ToList();
        }
    }
}