using System;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archiving.Abstractions;
using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Primitives;
using GalaxyMerge.Services.Abstractions;

namespace GalaxyMerge.Services
{
    public class Archiver : IArchiver, IDisposable
    {
        private readonly IGalaxyRepository _galaxyRepository;
        private readonly IDataRepository _dataRepository;
        private readonly IArchiveRepository _archiveRepository;

        public Archiver(IGalaxyRepository galaxyRepository, IDataRepository dataRepository, IArchiveRepository archiveRepository)
        {
            _galaxyRepository = galaxyRepository;
            _dataRepository = dataRepository;
            _archiveRepository = archiveRepository;
        }

        public void Dispose()
        {
            _dataRepository?.Dispose();
            _archiveRepository?.Dispose();
        }

        public void Archive(int objectId, bool force = false, int? changeLogId = null)
        {
            var gObject = _dataRepository.Objects.Find(objectId);
            ArchiveObject(gObject, force, changeLogId);
        }
        
        public void Archive(GObject gObject, bool force = false, int? changeLogId = null)
        {
            ArchiveObject(gObject, force, changeLogId);
        }
        
        private void ArchiveObject(GObject gObject, bool force = false, int? changeLogId = null)
        {
            if (!_archiveRepository.Objects.Exists(gObject.ObjectId))
                AddArchiveObject(gObject, changeLogId);

            //todo add is latest var isLatest = _archiveRepository.IsLatest();
            if (!force) return;
            UpdateArchiveObject(gObject, changeLogId);
        }

        private void AddArchiveObject(GObject gObject, int? changeLogId)
        {
            var template = Enumeration.FromId<Template>(gObject.TemplateId);
            if (template == null) throw new InvalidOperationException("Cannot Archive Unknown Template Type");

            var archiveObject = new ArchiveObject(gObject.ObjectId, gObject.TagName, gObject.ConfigVersion, template);

            var data = gObject.IsSymbol ? GetSymbolData(gObject.TagName) : GetObjectData(gObject.TagName);
            archiveObject.AddEntry(data, changeLogId);

            _archiveRepository.Objects.Add(archiveObject);
            _archiveRepository.Save();
        }

        private void UpdateArchiveObject(GObject gObject, int? changeLogId)
        {
            var archiveObject = _archiveRepository.Objects.FindInclude(gObject.ObjectId);

            if (gObject.TagName != archiveObject.TagName)
                archiveObject.UpdateTagName(gObject.TagName);

            if (gObject.ConfigVersion != archiveObject.Version)
                archiveObject.UpdateVersion(gObject.ConfigVersion);

            var data = gObject.IsSymbol ? GetSymbolData(gObject.TagName) : GetObjectData(gObject.TagName);
            archiveObject.AddEntry(data, changeLogId);

            _archiveRepository.Objects.Update(archiveObject);
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