using System;
using System.Collections.Generic;
using System.ServiceModel;
using GalaxyMerge.Archestra.Abstractions;
using GalaxyMerge.Archive.Abstractions;
using GalaxyMerge.Contracts;
using GalaxyMerge.Contracts.Data;
using GalaxyMerge.Contracts.Services;

namespace GalaxyMerge.Services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class GalaxyManager : IGalaxyService
    {
        private readonly IGalaxyRegistry _galaxyRegistry;
        
        public GalaxyManager(IGalaxyRegistry galaxyRegistry)
        {
            _galaxyRegistry = galaxyRegistry;
        }

        public GalaxyObject GetObject(string galaxyName, string tagName)
        {
            /*var galaxy = GetGalaxyRepository(galaxyName);
            var cacheRepository = _archiveRepositoryFactory.Create(galaxy);
            var cacheObject = cacheRepository.Find(tagName); //TODO should probably just have repo cache object if not found, so that this should always return data
            var xml = XElement.Load(new MemoryStream(cacheObject.CompressedData.Decompress()));
            var galaxyObject = new GalaxyObject {TagName = "Test"};
            return galaxyObject.DeserializeXml(xml.ToString());*/
            return null;
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

        public void CreateTemplate(string galaxyName, string tagName)
        {
            throw new NotImplementedException();
        }

        public void CreateTemplates(string galaxyName, IEnumerable<string> tagNames)
        {
            throw new NotImplementedException();
        }

        public void DeleteTemplate(string galaxyName, string tagName)
        {
            throw new NotImplementedException();
        }

        public void DeleteTemplates(string galaxyName, IEnumerable<string> tagNames)
        {
            throw new NotImplementedException();
        }
        
        private IGalaxyRepository GetGalaxyRepository(string galaxyName)
        {
            var clientUserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            var galaxy = _galaxyRegistry.GetGalaxy(galaxyName, clientUserName);

            if (galaxy == null)
                throw new InvalidOperationException(
                    $"Could not find registration for galaxy '{galaxyName}' and user '{clientUserName}'." +
                    $" This may because the galaxy has not been registered or the galaxy does not exist.");

            return galaxy;
        }

        private static void ValidateUserAccess()
        {
            var clientUserName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            //TODO Check user roles either via database or from current galaxy
        }
    }
}