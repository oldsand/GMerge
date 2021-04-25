using System;
using System.Linq;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Services
{
    public class GalaxyArchiver
    {
        private readonly IGalaxyRepository _galaxyRepository;
        private readonly IObjectRepository _objectRepository;
        private readonly IArchiveRepository _archiveRepository;

        public GalaxyArchiver(IGalaxyRepository galaxyRepository, IObjectRepository objectRepository, IArchiveRepository archiveRepository)
        {
            _galaxyRepository = galaxyRepository ?? throw new ArgumentNullException(nameof(galaxyRepository), "Value cannot be null");
            _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository), "Value cannot be null");
            _archiveRepository = archiveRepository ?? throw new ArgumentNullException(nameof(archiveRepository), "Value cannot be null");
        }

        public void Archive(string tagName)
        {
            var obj = _objectRepository.FindInclude(x => x.TagName == tagName, g => g.Template);
            ArchiveInternal(obj);
        }

        public void Archive(int objectId)
        {
            var obj = _objectRepository.FindInclude(x => x.ObjectId == objectId, x => x.Template);
            ArchiveInternal(obj);
        }

        public bool UpToDate(string tagName)
        {
            var gObject = _objectRepository.FindInclude(x => x.TagName == tagName, x => x.ChangeLogs);
            var entry = _archiveRepository.GetLatest(tagName);
            
            var lastCheckInDate = gObject.ChangeLogs
                .OrderByDescending(x => x.ChangeDate)
                .FirstOrDefault(x => x.OperationId == 0)?.ChangeDate;

            return gObject.ConfigVersion == entry.Version && lastCheckInDate <= entry.Created;
        }
        
        private void ArchiveInternal(GObject obj)
        {
            var data = obj.Template.TagName == "$Symbol" ? GetSymbolData(obj.TagName) : GetObjectData(obj.TagName);

            var entry = new ArchiveEntry(obj.ObjectId, obj.TagName, obj.ConfigVersion, obj.Template.TagName, data);
            _archiveRepository.AddEntry(entry);
            _archiveRepository.Save();
        }

        private byte[] GetObjectData(string tagName)
        {
            var galaxyObject = _galaxyRepository.GetObject(tagName);
            return galaxyObject.ToXml().ToByteArray();
        }

        private byte[] GetSymbolData(string tagName)
        {
            var galaxySymbol = _galaxyRepository.GetSymbol(tagName);
            return galaxySymbol.ToXml().ToByteArray();
        }
    }
}