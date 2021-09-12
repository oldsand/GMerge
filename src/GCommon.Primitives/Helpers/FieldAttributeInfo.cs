using System;

namespace GCommon.Primitives.Helpers
{
    public class FieldAttributeInfo : PrimitiveInfo
    {
        public const string AttributeName = "UserAttrData";
        private const string DefaultValue = "<AttrXML/>";
        
        public FieldAttributeInfo(ArchestraAttribute attribute) : base(attribute, DefaultValue)
        {
            if (attribute.Name != AttributeName)
                throw new InvalidOperationException($"Attribute name is not expected value' {AttributeName}'");
        }
    }
}