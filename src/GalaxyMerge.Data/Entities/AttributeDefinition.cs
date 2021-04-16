
// EF Core entity class. Only EF should be instantiating and setting properties.
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace GalaxyMerge.Data.Entities
{
    public class AttributeDefinition
    {
        internal AttributeDefinition()
        {
        }

        public int AttributeDefinitionId { get; private set; }
        public int PrimitiveDefinitionId { get; private set; }
        public string AttributeName { get; private set; }
        public short AttributeId { get; private set; }
        public bool HasConfigSetHandler { get; private set; }
        public short DataTypeId { get; private set; }
        public short SecurityClassificationId { get; private set; }
        public int AttributeCategoryId { get; private set; }
        public bool IsLocked { get; private set; }
        public object RawValue { get; private set; }
        public bool IsArray { get; private set; }

        public PrimitiveDefinition PrimitiveDefinition { get; private set; }
    }
}