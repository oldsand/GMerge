using System.Collections.Generic;
using System.Linq;
using GalaxyMerge.Data.Abstractions;
using GalaxyMerge.Data.Base;
using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalaxyMerge.Data.Repositories
{
    internal class DefinitionReadOnlyRepository : ReadOnlyRepository<TemplateDefinition>, IDefinitionReadOnlyRepository
    {
        internal DefinitionReadOnlyRepository(DbContext context) : base(context)
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