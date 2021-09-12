using System;

namespace GCommon.Primitives.Helpers
{
    public class ExtensionInfo : PrimitiveInfo
    {
        public const string AttributeName = "Extensions";
        private const string DefaultValue = "<ExtensionInfo><ObjectExtension/><AttriuteExtension/></ExtensionInfo>";
        
        public ExtensionInfo(ArchestraAttribute attribute) : base(attribute, DefaultValue)
        {
            if (attribute.Name != AttributeName)
                throw new InvalidOperationException($"Attribute name is not expected value' {AttributeName}'");
        }
    }
}