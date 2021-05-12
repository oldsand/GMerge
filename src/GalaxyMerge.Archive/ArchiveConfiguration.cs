using System.Collections.Generic;
using GalaxyMerge.Archive.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Archive
{
    public class ArchiveConfiguration
    {
        public string GalaxyName { get; private set; }
        public string FileName { get; private set; }
        public string ConnectionString { get; private set; }
        internal DbContextOptions<ArchiveContext> ContextOptions { get; private set; }
        public GalaxyInfo GalaxyInfo { get; private set; }
        public List<EventSetting> EventSettings { get; private set; }
        public List<InclusionSetting> InclusionSettings { get; private set; }
    }
}