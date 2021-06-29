using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using GalaxyMerge.Core.Utilities;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    public class DefinitionRepository : Repository<TemplateDefinition>, IDefinitionRepository
    {
        public DefinitionRepository(string galaxyName) 
            : base(GalaxyContext.Create(DbStringBuilder.BuildGalaxy(galaxyName)))
        {
        }
        
        public DefinitionRepository(string hostName, string galaxyName) 
            : base(GalaxyContext.Create(DbStringBuilder.BuildGalaxy(hostName, galaxyName)))
        {
        }
        
        public DefinitionRepository(DbConnectionStringBuilder connectionStringBuilder) 
            : base(GalaxyContext.Create(connectionStringBuilder.ConnectionString))
        {
        }

        public TemplateDefinition FindIncludeAll(string templateName)
        {
            return Set.Include(t => t.PrimitiveDefinitions)
                .ThenInclude(p => p.AttributeDefinitions)
                .SingleOrDefault(t => t.TagName == templateName);
        }

        public IEnumerable<string> GetExtensionAttributes(string templateName, string extensionType)
        {
            throw new System.NotImplementedException();
        }
    }
}