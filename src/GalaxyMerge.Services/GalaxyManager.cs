using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Principal;
using System.ServiceModel;
using System.Xml.Linq;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archestra.Entities;
using GalaxyMerge.Archive.Repositories;
using GalaxyMerge.Contracts;
using GalaxyMerge.Core.Extensions;
using GalaxyMerge.Data.Repositories;

namespace GalaxyMerge.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class GalaxyManager : IGalaxyService
    {
        private readonly IGalaxyRegistry _galaxyRegistry;
        private IGalaxyRepository _serviceGrSession;
        private IGalaxyRepository _clientGrSession;

        public GalaxyManager(IGalaxyRegistry galaxyRegistry)
        {
            _galaxyRegistry = galaxyRegistry;
        }

        public bool Connect(string galaxyName)
        {
            ConnectServiceSession(galaxyName);
            ConnectClientSession(galaxyName);
            
            return _clientGrSession.Name == galaxyName && _clientGrSession.Connected;
        }

        public GalaxyObject GetObject(int objectId)
        {
            //todo should probably split this out to archive manager to allow client cde to determine where to get object from.
            using var archiveRepository = new ArchiveRepository(_clientGrSession.Name);
            var exists = archiveRepository.ObjectExists(objectId);
            if (exists)
            {
                var latest = archiveRepository.GetLatestEntry(objectId);
                var xml = XElement.Load(new MemoryStream(latest.CompressedData.Decompress()));
                var galaxyObject = new GalaxyObject().FromXml(xml);
                return (GalaxyObject) galaxyObject;
            }
            
            using var objectRepository = new ObjectRepository(_clientGrSession.Name);
            var tagName = objectRepository.GetTagName(objectId);
            return (GalaxyObject) _clientGrSession.GetObject(tagName);
        }

        public GalaxyObject GetObjects(string tagName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GalaxyObject> GetObjects(IEnumerable<int> objectIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GalaxyObject> GetObjects(IEnumerable<string> tagNames)
        {
            throw new NotImplementedException();
        }


        private void ConnectClientSession(string galaxyName)
        {
            _clientGrSession = null;
            
            var clientUserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            if (!_galaxyRegistry.IsRegistered(galaxyName, clientUserName))
                RegisterGalaxyToClient(galaxyName, clientUserName);
            
            var galaxyRepository = _galaxyRegistry.GetGalaxy(galaxyName, clientUserName);
            if (galaxyRepository == null)
                throw new InvalidOperationException(
                    $"Cannot find registered galaxy with name '{galaxyName}' for current service name '{clientUserName}'");
            
            galaxyRepository.SynchronizeClient();
            _clientGrSession = galaxyRepository;
        }

        private void RegisterGalaxyToClient(string galaxyName, string userName)
        {
            //todo not really sure if this is the right exception to throw here
            if (!_serviceGrSession.UserIsAuthorized(userName))
                throw new SecurityException("User does not have access to the specified galaxy");
            
            _galaxyRegistry.Register(galaxyName, userName);
        }

        private void ConnectServiceSession(string galaxyName)
        {
            _serviceGrSession = null;
            
            var serviceUser = WindowsIdentity.GetCurrent();
            var galaxyRepository = _galaxyRegistry.GetGalaxy(galaxyName, serviceUser.Name);

            _serviceGrSession = galaxyRepository ?? throw new InvalidOperationException(
                $"Cannot find registered galaxy with name '{galaxyName}' for current service name '{serviceUser.Name}'");
        }
    }
}