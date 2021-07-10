using System.Collections.Generic;
using System.Linq;
using GCommon.Data.Abstractions;
using GCommon.Data.Base;
using GCommon.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GCommon.Data.Repositories
{
    internal class DefinitionRepository : ReadOnlyRepository<TemplateDefinition>, IDefinitionRepository
    {
        internal DefinitionRepository(DbContext context) : base(context)
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