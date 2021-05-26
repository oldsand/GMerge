using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArchestrA.GRAccess;
using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Archestra
{
    public class GalaxyRepositoryFactory : IGalaxyRepositoryFactory
    {
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
            
            var galaxies = grAccess.QueryGalaxies(host);
            ResultHandler.Handle(grAccess.CommandResult, Environment.MachineName);
            
            foreach (IGalaxy galaxy in galaxies)
                yield return new GalaxyRepository(grAccess, galaxy);
        }
    }
}