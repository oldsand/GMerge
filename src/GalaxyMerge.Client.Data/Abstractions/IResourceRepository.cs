using System;
using System.Collections.Generic;
using GalaxyMerge.Client.Data.Entities;

namespace GalaxyMerge.Client.Data.Abstractions
{
    public interface IResourceRepository : IDisposable
    {
        GalaxyResource Get(int id);
        GalaxyResource Get(string name);
        IEnumerable<GalaxyResource> GetAll();
        void Add(GalaxyResource resource);
        void Remove(GalaxyResource resource);
        void Update(GalaxyResource resource);
        void Save();
    }
}