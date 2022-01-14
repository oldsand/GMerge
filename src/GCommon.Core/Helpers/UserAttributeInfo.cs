using System;

namespace GCommon.Core.Helpers
{
    public class UserAttributeInfo : PrimitiveInfo
    {
        public const string AttributeName = "UDAs";
        private const string DefaultValue = "<UDAInfo></UDAInfo>";
        
        public UserAttributeInfo(ArchestraAttribute attribute) : base(attribute, DefaultValue)
        {
            if (attribute.Name != AttributeName)
                throw new InvalidOperationException($"Attribute name is not expected value' {AttributeName}'");
        }
    }
}