using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Principal;
using System.ServiceModel;
using System.Xml.Linq;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Abstractions;
using GalaxyMerge.Contracts.Data;
using GalaxyMerge.Contracts.Services;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class GalaxyManager : IGalaxyService
    {
        private readonly IGalaxyRegistry _galaxyRegistry;
        private readonly IArchiveRepository _archiveRepository;

        public GalaxyManager(string galaxyName)
        {
            //todo figure out how to pass this to constructor from client using WCF service.
            // this way we can construct dependencies.
        }

        public GalaxyManager(IGalaxyRegistry galaxyRegistry, IArchiveRepository archiveRepository)
        {
            _galaxyRegistry = galaxyRegistry;
            _archiveRepository = archiveRepository;
        }

        public GalaxyObject GetObject(string galaxyName, string tagName)
        {
            var objectId = 0; //todo need object repo here.
            var hasEntries = _archiveRepository.HasEntries(objectId);
            if (!hasEntries)
            {
                var galaxyRepository = GetClientGalaxyRepository(galaxyName);
                var objectRepository = new ObjectRepository(ConnectionStringBuilder.BuildGalaxyConnection(galaxyName));
                var archiver = new GalaxyArchiver(galaxyRepository, objectRepository, _archiveRepository);
                archiver.Archive(tagName);
            }

            var latest = _archiveRepository.GetLatestEntry(objectId);
            
            var xml = XElement.Load(new MemoryStream(latest.CompressedData.Decompress()));
            var galaxyObject = new GalaxyObject().FromXml(xml);
            return (GalaxyObject) galaxyObject;
        }

        public IEnumerable<GalaxyObject> GetObjects(string galaxyName, IEnumerable<string> tagNames)
        {
            throw new NotImplementedException();
        }

        public void UpdateObject(string galaxyName, GalaxyObject template)
        {
            throw new NotImplementedException();
        }

        public void UpdateObjects(string galaxyName, IEnumerable<GalaxyObject> templates)
        {
            throw new NotImplementedException();
        }

        public void CreateObject(string galaxyName, string tagName)
        {
            throw new NotImplementedException();
        }

        public void CreateObject(string galaxyName, IEnumerable<string> tagNames)
        {
            throw new NotImplementedException();
        }

        public void DeleteObject(string galaxyName, string tagName)
        {
            throw new NotImplementedException();
        }

        public void DeleteObjects(string galaxyName, IEnumerable<string> tagNames)
        {
            throw new NotImplementedException();
        }
        
        private IGalaxyRepository GetClientGalaxyRepository(string galaxyName)
        {
            var clientUserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            if (!_galaxyRegistry.IsRegistered(galaxyName, clientUserName))
                RegisterGalaxyToClient(galaxyName);
            
            var galaxy = _galaxyRegistry.GetGalaxy(galaxyName, clientUserName);
            galaxy.SynchronizeClient();
            return galaxy;
        }

        private void RegisterGalaxyToClient(string galaxyName)
        {
            var clientUserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            var serviceUser = WindowsIdentity.GetCurrent();
            var galaxy = _galaxyRegistry.GetGalaxy(galaxyName, serviceUser.Name);

            if (galaxy == null)
                throw new InvalidOperationException(
                    $"Cannot find registered galaxy with name '{galaxyName}' under current service name '{serviceUser.Name}'");

            if (galaxy.UserIsAuthorized(clientUserName))
                _galaxyRegistry.Register(galaxyName, clientUserName);
            else
            {
                throw new SecurityException("User does not have access to the specified galaxy");
            }
        }
    }
}