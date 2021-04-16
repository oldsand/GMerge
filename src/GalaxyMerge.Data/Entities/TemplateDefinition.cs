using System.Collections.Generic;

// EF Core entity class. Only EF should be instantiating and setting properties.
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace GalaxyMerge.Data.Entities
{
    public class TemplateDefinition
    {
        internal TemplateDefinition()
        {
        }
        
        public int TemplateDefinitionId { get; private set; }
        public int ObjectId { get;  private set; }
        public string TagName { get;  private set; }
        public short CategoryId { get;  private set; }
        public string Codebase { get;  private set; }
        public IEnumerable<GObject> Derivations { get; private set; }

        public IEnumerable<PrimitiveDefinition> PrimitiveDefinitions { get; set; }
    }
}