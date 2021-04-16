using System.Collections.Generic;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IDefinitionRepository : IRepository<TemplateDefinition>
    {
        TemplateDefinition FindIncludeAll(string templateName);
        IEnumerable<string> GetExtensionAttributes(string templateName, string extensionType);
    }
}