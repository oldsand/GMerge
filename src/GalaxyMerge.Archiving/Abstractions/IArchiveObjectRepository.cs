using System;
using System.Collections.Generic;
using GalaxyMerge.Archiving.Entities;

namespace GalaxyMerge.Archiving.Abstractions
{
    public interface IArchiveObjectRepository : IDisposable
    {
        bool Exists(int objectId);
        ArchiveObject Get(int objectId);
        IEnumerable<ArchiveObject> GetAll();
        IEnumerable<ArchiveObject> FindByTagName(string tagName);
        void Upsert(ArchiveObject archiveObject);
        void Delete(int objectId);
    }
}