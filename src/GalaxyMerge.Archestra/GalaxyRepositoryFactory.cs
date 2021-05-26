using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArchestrA.GRAccess;
using GalaxyMerge.Archestra.Abstractions;
using NLog;

namespace GalaxyMerge.Archestra
{
    public class GalaxyRepositoryFactory : IGalaxyRepositoryFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public IGalaxyRepository Create(string galaxyName)
        {
            return new GalaxyRepository(galaxyName);
        }

        public Task<IGalaxyRepository> CreateAsync(string galaxyName, CancellationToken token)
        {
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                return Create(galaxyName);
            }, token);
        }

        public IEnumerable<IGalaxyRepository> CreateAll()
        {
            var host = Environment.MachineName;
            var grAccess = new GRAccessAppClass();
            
            Logger.Debug("Querying for galaxies not host {Host}", host);
            var galaxies = grAccess.QueryGalaxies(host);
            ResultHandler.Handle(grAccess.CommandResult, Environment.MachineName);

            foreach (IGalaxy galaxy in galaxies)
            {
                Logger.Debug("Creating repository instance for galaxy '{Galaxy}'", galaxy.Name);
                yield return new GalaxyRepository(grAccess, galaxy);
            }
        }
    }
}