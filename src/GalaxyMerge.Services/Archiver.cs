using System;
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

        //todo need to rethink how this is possible if tag name is not unique
        public void Archive(string tagName)
        {
            var obj = _objectRepository.FindInclude(x => x.TagName == tagName, g => g.Template);
            UpsertObject(obj);
            AddEntry(obj);
        }

        public void Archive(int objectId)
        {
            var obj = _objectRepository.FindInclude(x => x.ObjectId == objectId, x => x.Template);
            UpsertObject(obj);
            AddEntry(obj);
        }

        //todo figure out where this should go.
        /*public bool UpToDate(string tagName)
        {
            var gObject = _objectRepository.FindInclude(x => x.TagName == tagName, x => x.ChangeLogs);
            var entry = _archiveRepository.GetLatest(tagName);
            
            var lastCheckInDate = gObject.ChangeLogs
                .OrderByDescending(x => x.ChangeDate)
                .FirstOrDefault(x => x.OperationId == 0)?.ChangeDate;

            return gObject.ConfigVersion == entry.Version && lastCheckInDate <= entry.CreatedOn;
        }*/

        private void UpsertObject(GObject obj)
        {
            var archiveObject = _archiveRepository.GetObject(obj.ObjectId);
            
            if (_archiveRepository.ObjectExists(obj.ObjectId))
                _archiveRepository.UpdateObject(archiveObject);
            else
                _archiveRepository.AddObject(archiveObject);
            
            _archiveRepository.Save();
        }
        
        private void AddEntry(GObject obj)
        {
            var data = obj.Template.TagName == "$Symbol" ? GetSymbolData(obj.TagName) : GetObjectData(obj.TagName);

            var archiveObject = _archiveRepository.GetObject(obj.ObjectId);
            archiveObject.AddEntry(data);

            if (obj.TagName != archiveObject.TagName)
                archiveObject.UpdateTagName(obj.TagName);
            
            if (obj.ConfigVersion != archiveObject.Version)
                archiveObject.UpdateVersion(obj.ConfigVersion);
            
            _archiveRepository.UpdateObject(archiveObject);
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