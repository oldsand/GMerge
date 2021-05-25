using System;
using System.Linq;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Core;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Repositories;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Services
{
    public class ArchiveProcessor
    {
        private readonly string _galaxyName;
        private readonly IGalaxyRepository _galaxyRepository;

        public ArchiveProcessor(IGalaxyRepository galaxyRepository)
        {
            _galaxyRepository = galaxyRepository ?? throw new ArgumentNullException(nameof(galaxyRepository), "Value cannot be null");

            if (!galaxyRepository.Connected)
                throw new ArgumentException("Galaxy Repository must be connect in order to initialize this object",
                    nameof(galaxyRepository));
            
            _galaxyName = _galaxyRepository.Name;
        }

        public void Archive(int objectId, int? changeLogId = null, bool forceArchive = false)
        {
            var gObject = GetGObject(objectId);
            ArchiveObject(gObject, changeLogId, forceArchive);
        }
        
        public void Archive(string tagName, int? changeLogId = null, bool forceArchive = false)
        {
            var gObject = GetGObject(tagName);
            ArchiveObject(gObject, changeLogId, forceArchive);
        }

        private void ArchiveObject(GObject gObject, int? changeLogId, bool forceArchive)
        {
            if (Exists(gObject.ObjectId))
            {
                if (!forceArchive && IsLatest(gObject)) return;
                UpdateArchiveObject(gObject, changeLogId);
                return;
            }
            
            AddArchiveObject(gObject, changeLogId);
        }

        private void AddArchiveObject(GObject gObject, int? changeLogId)
        {
            using var archiveRepo = new ArchiveRepository(_galaxyName);
            
            var template = Enumeration.FromId<Template>(gObject.TemplateId);
            if (template == null) throw new InvalidOperationException("Cannot Archive Unknown Template Type");
            
            var archiveObject = new ArchiveObject(gObject.ObjectId, gObject.TagName, gObject.ConfigVersion, template);
            
            var data = IsSymbol(gObject) ? GetSymbolData(gObject.TagName) : GetObjectData(gObject.TagName);
            archiveObject.AddEntry(data, changeLogId);
            
            archiveRepo.AddObject(archiveObject);
            archiveRepo.Save();
        }
        
        private void UpdateArchiveObject(GObject gObject, int? changeLogId)
        {
            using var archiveRepo = new ArchiveRepository(_galaxyName);

            var archiveObject = archiveRepo.GetObjectIncludeEntries(gObject.ObjectId);

            var data = IsSymbol(gObject) ? GetSymbolData(gObject.TagName) : GetObjectData(gObject.TagName);
            archiveObject.AddEntry(data, changeLogId);
            
            if (gObject.TagName != archiveObject.TagName)
                archiveObject.UpdateTagName(gObject.TagName);
            
            if (gObject.ConfigVersion != archiveObject.Version)
                archiveObject.UpdateVersion(gObject.ConfigVersion);
            
            archiveRepo.UpdateObject(archiveObject);
            archiveRepo.Save();
        }

        private static bool IsSymbol(GObject gObject)
        {
            return Enumeration.FromId<Template>(gObject.TemplateId).Equals(Template.Symbol);
        }

        //todo this still needs work. we can't assume comparing version/date to check in operation always will work.
        private bool IsLatest(GObject gObject)
        {
            using var archiveRepo = new ArchiveRepository(_galaxyName);
            var latest = archiveRepo.GetLatestEntry(gObject.ObjectId);
            
            using var changeLogRepo = new ChangeLogRepository(_galaxyName);
            var changeLog = changeLogRepo.GetLatestByOperation(gObject.ObjectId, Operation.CheckInSuccess);
            
            return latest != null && changeLog.ConfigurationVersion == latest.Version && changeLog.ChangeDate <= latest.ArchivedOn;
        }

        private bool Exists(int objectId)
        {
            using var repo = new ArchiveRepository(_galaxyName);
            return repo.ObjectExists(objectId);
        }
        
        private GObject GetGObject(int objectId)
        {
            using var objectRepo = new ObjectRepository(_galaxyName);
            return objectRepo.FindInclude(x => x.ObjectId == objectId, x => x.Template);
        }
        
        private GObject GetGObject(string tagName)
        {
            using var objectRepo = new ObjectRepository(_galaxyName);
            return objectRepo.FindAllInclude(x => x.TagName == tagName, x => x.Template).FirstOrDefault();
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