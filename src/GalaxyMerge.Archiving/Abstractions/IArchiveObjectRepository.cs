using System.Collections.Generic;
using GalaxyMerge.Archiving.Entities;

namespace GalaxyMerge.Archiving.Abstractions
{
    public interface IArchiveObjectRepository
    {
        bool Exists(int objectId);
        ArchiveObject Find(int objectId);
        IEnumerable<ArchiveObject> Find(string tagName);
        ArchiveObject FindInclude(int objectId);
        IEnumerable<ArchiveObject> GetAll();
        void Add(ArchiveObject archiveObject);
        void Remove(ArchiveObject archiveObject);
        void Remove(int objectId);
        void Update(ArchiveObject archiveObject);
    }
}