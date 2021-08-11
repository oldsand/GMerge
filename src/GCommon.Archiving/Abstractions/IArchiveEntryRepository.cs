using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GCommon.Primitives;

namespace GCommon.Archiving.Abstractions
{
    public interface IArchiveEntryRepository : IDisposable
    {
        IEnumerable<ArchiveEntry> GetAll();
        IEnumerable<ArchiveEntry> Find(Expression<Func<ArchiveEntry, bool>> predicate);
    }
}