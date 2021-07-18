using System;
using System.Collections.Generic;
using System.Linq;
using ArchestrA.GRAccess;
using GServer.Archestra.Abstractions;
using GServer.Archestra.Extensions;

namespace GServer.Archestra
{
    public class GalaxyAccess : IGalaxyCreator, IGalaxyDestroyer, IGalaxyFinder
    {
        private readonly GRAccessAppClass _access;
        private const string BaseGalaxyTemplate = "Base_Application_Server.cab";

        public GalaxyAccess()
        {
            _access = new GRAccessAppClass();
        }
        
        public void Create(string galaxyName)
        {
            if (galaxyName == null) throw new ArgumentNullException(nameof(galaxyName), "galaxyName can not be null");
            
            var template = _access.ListCreateGalaxyTemplates().ToList().SingleOrDefault(x => x == BaseGalaxyTemplate);

            if (string.IsNullOrEmpty(template))
                throw new InvalidOperationException($"Could not find base template to create galaxy {galaxyName}");

            _access.CreateGalaxyFromTemplate(template, galaxyName);
            _access.CommandResult.Process();
        }
        
        public void Destroy(string galaxyName)
        {
            if (galaxyName == null) throw new ArgumentNullException(nameof(galaxyName), "galaxyName can not be null");
            
            _access.DeleteGalaxy(galaxyName);
            _access.CommandResult.Process();
        }

        public bool Exists(string galaxyName)
        {
            if (galaxyName == null) throw new ArgumentNullException(nameof(galaxyName), "galaxyName can not be null");
            return _access.QueryGalaxies()[galaxyName] != null;
        }

        public IEnumerable<string> Find()
        {
            var galaxies = _access.QueryGalaxies();
            _access.CommandResult.Process();

            foreach (IGalaxy galaxy in galaxies)
                yield return galaxy.Name;
        }
    }
}