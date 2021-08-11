using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GCommon.Archiving.Abstractions;
using GCommon.Primitives;

namespace GCommon.Archiving.Repositories
{
    internal class ArchiveEntryRepository : IArchiveEntryRepository
    {
        private readonly ArchiveContext _context;

        public ArchiveEntryRepository(ArchiveContext context)
        {
            _context = context;
        }
        
        public IEnumerable<ArchiveEntry> GetAll()
        {
            return _context.Entries.ToList();
        }

        public IEnumerable<ArchiveEntry> Find(Expression<Func<ArchiveEntry, bool>> predicate)
        {
            return _context.Entries.Where(predicate).ToList();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}