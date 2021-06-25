using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaxyMerge.Client.Data.Entities;

namespace GalaxyMerge.Client.Data.Abstractions
{
    public interface IResourceRepository : IDisposable
    {
        ResourceEntry Get(int id);
        ResourceEntry Get(string name);
        IEnumerable<ResourceEntry> GetAll();
        IEnumerable<string> GetNames();
        Task<IEnumerable<string>> GetNamesAsync();
        Task<IEnumerable<ResourceEntry>> GetAllAsync();
        void Add(ResourceEntry resourceEntry);
        void Remove(ResourceEntry resourceEntry);
        void Update(ResourceEntry resourceEntry);
        void Save();
    }
}