using System.Collections.Generic;
using GalaxyMerge.Data.Base;
using GalaxyMerge.Data.Entities;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IDefinitionReadOnlyRepository : IReadOnlyRepository<TemplateDefinition>
    {
        TemplateDefinition FindIncludeAll(string templateName);
        IEnumerable<string> GetExtensionAttributes(string templateName, string extensionType);
    }
}