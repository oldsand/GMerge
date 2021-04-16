using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Entities;
using GalaxyMerge.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    public class DefinitionRepository : Repository<TemplateDefinition>, IDefinitionRepository
    {
        public DefinitionRepository(string connectionString) : base(ContextCreator.Create(connectionString))
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