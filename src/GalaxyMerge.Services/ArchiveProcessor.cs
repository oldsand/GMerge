using System;
using System.Linq;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Abstractions;
using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Services
{
    public class ArchiveProcessor
    {
        private readonly string _galaxyName;
        private readonly IGalaxyRepository _galaxyRepository;

        public ArchiveProcessor(IGalaxyRepository galaxyRepository)
        {
            _galaxyRepository = galaxyRepository ?? throw new ArgumentNullException(nameof(galaxyRepository), "Value cannot be null");
            _galaxyName = _galaxyRepository.Name;
        }

        public void Archive(int objectId, bool forceArchive = false)
        {
            using var objectRepo = new ObjectRepository(_galaxyName);
            var gObject = objectRepo.FindInclude(x => x.ObjectId == objectId, x => x.Template);

            using var archiveRepo = new ArchiveRepository(_galaxyName);
            var archiveObject = GetArchiveObject(gObject, archiveRepo);

            if (!forceArchive && IsLatest(archiveObject)) return;

            var data = gObject.Template.TagName == "$Symbol" 
                ? GetSymbolData(gObject.TagName) : GetObjectData(gObject.TagName);
            
            archiveObject.AddEntry(data);
            archiveRepo.Save();
        }

        private static ArchiveObject GetArchiveObject(GObject gObject, IArchiveRepository archiveRepo)
        {
            if (archiveRepo.ObjectExists(gObject.ObjectId))
                return archiveRepo.GetObject(gObject.ObjectId);
            
            var archiveObject = new ArchiveObject(gObject.ObjectId, gObject.TagName, gObject.ConfigVersion,
                    Enumeration.FromId<Template>(gObject.Template.TemplateDefinitionId));
            archiveRepo.AddObject(archiveObject);
            return archiveObject;
        }

        private bool IsLatest(ArchiveObject archiveObject)
        {
            using var changeLogRepo = new ChangeLogRepository(_galaxyName);
            var changeLog = changeLogRepo.GetLatestByOperation(archiveObject.ObjectId, Operation.CheckInSuccess);

            var entry = archiveObject.Entries.OrderByDescending(x => x.ArchivedOn).FirstOrDefault();

            return changeLog.ConfigurationVersion == entry?.Version && changeLog.ChangeDate <= entry.ArchivedOn;
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