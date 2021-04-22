using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GalaxyMerge.Archive.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive.Repositories
{
    public class ArchiveRepository : IArchiveRepository
    {
        private readonly ArchiveContext _context;
        
        public ArchiveRepository(string galaxyName)
        {
            var connectionString = ConnectionStringBuilder.BuildArchiveConnection(galaxyName);
            var options = new DbContextOptionsBuilder<ArchiveContext>().UseSqlite(connectionString).Options;
            _context = new ArchiveContext(options);
            _context.Database.EnsureCreated();
        }

        public ArchiveInfo GetInfo()
        {
            return _context.Info.SingleOrDefault();
        }

        public ArchiveEntry GetEntry(Guid id)
        {
            return _context.Entries.Find(id);
        }

        public IEnumerable<ArchiveEntry> FindByObjectId(int objectId)
        {
            return _context.Entries.Where(x => x.ObjectId == objectId);
        }

        public IEnumerable<ArchiveEntry> FindByTagName(string tagName)
        {
            return _context.Entries.Where(x => x.TagName == tagName);
        }

        public ArchiveEntry GetLatest(string tagName)
        {
            return _context.Entries.OrderByDescending(x => x.Created).FirstOrDefault(x => x.TagName == tagName);
        }

        public bool HasEntries(string tagName)
        {
            return _context.Entries.Any(x => x.TagName == tagName);
        }

        public void AddInfo(ArchiveInfo archiveInfo)
        {
            _context.Info.Add(archiveInfo);
        }

        public void RemoveInfo(ArchiveInfo archiveInfo)
        {
            _context.Info.Remove(archiveInfo);
        }

        public void UpdateInfo(ArchiveInfo archiveInfo)
        {
            _context.Info.Update(archiveInfo);
        }

        public void AddEntry(ArchiveEntry archiveEntry)
        {
            _context.Entries.Add(archiveEntry);
        }

        public void RemoveEntry(ArchiveEntry archiveEntry)
        {
            _context.Entries.Remove(archiveEntry);
        }

        public void UpdateEntry(ArchiveEntry archiveEntry)
        {
            _context.Entries.Update(archiveEntry);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}