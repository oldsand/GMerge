using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArchestrA.GRAccess;
using GalaxyMerge.Archestra.Abstractions;

namespace GalaxyMerge.Archestra
{
    public class GalaxyFinder : IGalaxyFinder
    {
        public bool Exists(string galaxyName)
        {
            var host = Environment.MachineName;
            var grAccess = new GRAccessAppClass();
            return grAccess.QueryGalaxies(host)[galaxyName] != null;
        }

        public Task<bool> ExistsAsync(string galaxyName, CancellationToken token)
        {
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                return Exists(galaxyName);
            }, token);
        }

        public IEnumerable<string> FindAll()
        {
            var host = Environment.MachineName;
            var grAccess = new GRAccessAppClass();
            
            var galaxies = grAccess.QueryGalaxies(host);
            ResultHandler.Handle(grAccess.CommandResult, Environment.MachineName);
            
            foreach (IGalaxy galaxy in galaxies)
                yield return galaxy.Name;
        }

        public Task<IEnumerable<string>> FindAllAsync(CancellationToken token)
        {
            return Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();
                return FindAll();

            }, token);
        }
    }
}