using System.Collections.Generic;
using GCommon.Data.Base;
using GCommon.Data.Entities;

namespace GCommon.Data.Abstractions
{
    public interface IDefinitionRepository : IReadOnlyRepository<TemplateDefinition>
    {
        TemplateDefinition FindIncludeAll(string templateName);
        IEnumerable<string> GetExtensionAttributes(string templateName, string extensionType);
    }
}