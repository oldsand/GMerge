using System;
using System.IO;
using System.Xml.Linq;
using GServer.Archestra.Abstractions;
using GServer.Archestra.Entities;
using GCommon.Archiving.Abstractions;
using GCommon.Archiving.Entities;
using GCommon.Core.Extensions;
using GCommon.Primitives;
using GServer.Services.Abstractions;

namespace GServer.Services
{
    public class GalaxyArchiver : IGalaxyArchiver, IDisposable
    {
        private readonly IGalaxyRepository _galaxyRepository;
        private readonly IArchiveRepository _archiveRepository;

        public GalaxyArchiver(IGalaxyRepository galaxyRepository, IArchiveRepository archiveRepository)
        {
            _galaxyRepository = galaxyRepository;
            _archiveRepository = archiveRepository;
        }

        public void Dispose()
        {
            _archiveRepository?.Dispose();
        }

        public void Archive(ArchiveObject archiveObject)
        {
            var updatedObject = archiveObject.Template == Template.Symbol
                ? ArchiveGalaxySymbol(archiveObject)
                : ArchiveGalaxyObject(archiveObject);

            _archiveRepository.Objects.Upsert(updatedObject);
            _archiveRepository.Save();
        }

        private ArchiveObject ArchiveGalaxySymbol(ArchiveObject archiveObject)
        {
            var entryData = XElement.Load(new MemoryStream(archiveObject.GetLatestEntry().CompressedData.Decompress()));
            var latest = new GalaxySymbol(archiveObject.TagName).FromXml(entryData);

            var current = _galaxyRepository.GetSymbol(archiveObject.TagName);
            
            //todo compute hash to determine equality between latest entry and current data
            
            var data = current.ToXml().ToByteArray();
            archiveObject.AddEntry(data);
            return archiveObject;
        }
        
        private ArchiveObject ArchiveGalaxyObject(ArchiveObject archiveObject)
        {
            var entryData = XElement.Load(new MemoryStream(archiveObject.GetLatestEntry().CompressedData.Decompress()));
            var latest = new GalaxyObject().FromXml(entryData);

            var current = _galaxyRepository.GetObject(archiveObject.TagName);
            
            //todo compute hash to determine equality between latest entry and current data
            
            var data = current.ToXml().ToByteArray();
            archiveObject.AddEntry(data);
            return archiveObject;
        }
    }
}