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
        private readonly IGalaxyDataProvider _galaxyDataProvider;
        private readonly IArchiveRepository _archiveRepository;

        public Archiver(IGalaxyRepository galaxyRepository, IGalaxyDataProvider galaxyDataProvider, IArchiveRepository archiveRepository)
        {
            _galaxyRepository = galaxyRepository;
            _galaxyDataProvider = galaxyDataProvider;
            _archiveRepository = archiveRepository;
        }

        public void Dispose()
        {
            _galaxyDataProvider?.Dispose();
            _archiveRepository?.Dispose();
        }

        public void Archive(int objectId, bool force = false, int? changeLogId = null)
        {
            var gObject = _galaxyDataProvider.Objects.Find(objectId);
            ArchiveObject(gObject, force, changeLogId);
        }
        
        public void Archive(GObject gObject, bool force = false, int? changeLogId = null)
        {
            ArchiveObject(gObject, force, changeLogId);
        }
        
        private void ArchiveObject(GObject gObject, bool force = false, int? changeLogId = null)
        {
            var template = Enumeration.FromId<Template>(gObject.TemplateId);
            if (template == null) throw new InvalidOperationException("Cannot Archive Unknown Template Type");

            var archiveObject = new ArchiveObject(gObject.ObjectId, gObject.TagName, gObject.ConfigVersion, template);

            var data = gObject.IsSymbol ? GetSymbolData(gObject.TagName) : GetObjectData(gObject.TagName);
            archiveObject.AddEntry(data, changeLogId);
            
            _archiveRepository.Objects.Upsert(archiveObject);
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