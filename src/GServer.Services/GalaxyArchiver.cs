using System;
using GServer.Archestra.Abstractions;
using GCommon.Archiving.Abstractions;
using GCommon.Extensions;
using GCommon.Core;
using GCommon.Core.Enumerations;
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
            
            _archiveRepository.UpsertObject(archiveObject);
            _archiveRepository.Save();
        }
        
        private byte[] GetArchiveData(ArchiveObject archiveObject)
        {
            return archiveObject.Template == Template.Symbol
                ? _galaxyRepository.GetGraphic(archiveObject.TagName).Serialize().ToByteArray()
                : _galaxyRepository.GetObject(archiveObject.TagName).Serialize().ToByteArray();
        }
    }
}