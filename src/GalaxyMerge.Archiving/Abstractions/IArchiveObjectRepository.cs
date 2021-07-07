using System;
using System.Collections.Generic;
using GalaxyMerge.Archiving.Entities;

namespace GalaxyMerge.Archiving.Abstractions
{
    public interface IArchiveObjectRepository : IDisposable
    {
        bool Exists(int objectId);
        ArchiveObject Get(int objectId);
        IEnumerable<ArchiveObject> Get(string tagName);
        IEnumerable<ArchiveObject> GetAll();
        void Add(ArchiveObject archiveObject);
        void Remove(int objectId);
        void Update(ArchiveObject archiveObject);
    }
}