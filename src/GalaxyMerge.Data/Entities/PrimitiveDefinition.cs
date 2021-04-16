using System.Collections.Generic;

// EF Core entity class. Only EF should be instantiating and setting properties.
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace GalaxyMerge.Data.Entities
{
    public class PrimitiveDefinition
    {
        internal PrimitiveDefinition()
        {
        }

        public int PrimitiveDefinitionId { get; private set; }
        public int TemplateDefinitionId { get; private set; }
        public short ParentPrimitiveId { get; private set; }
        public short PrimitiveId { get; private set; }
        public string PrimitiveName { get; private set; }
        public short ExecutionGroup { get; private set; }
        public bool IsVirtual { get; private set; }
        public bool SupportsDynamicAttributes { get; private set; }
        public int MajorVersion { get; private set; }

        public TemplateDefinition TemplateDefinition { get; private set; }
        public IEnumerable<AttributeDefinition> AttributeDefinitions { get; private set; }
    }
}