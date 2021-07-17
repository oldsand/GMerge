using System;
using GServer.Archestra.Abstractions;
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
            var data = GetArchiveData(archiveObject);
            
            archiveObject.Archive(data);
            
            _archiveRepository.Objects.Upsert(archiveObject);
            _archiveRepository.Save();
        }
        
        private byte[] GetArchiveData(ArchiveObject archiveObject)
        {
            return archiveObject.Template == Template.Symbol
                ? _galaxyRepository.GetGraphic(archiveObject.TagName).ToXml().ToByteArray()
                : _galaxyRepository.GetObject(archiveObject.TagName).ToXml().ToByteArray();
        }
    }
}