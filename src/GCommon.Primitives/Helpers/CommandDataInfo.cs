using System;
using System.Xml.Linq;

namespace GCommon.Primitives.Helpers
{
    public class CommandDataInfo : PrimitiveInfo
    {
        public const string AttributeName = "CmdData";
        public const string DefaultValue = "<CmdData><BooleanLabel></BooleanLabel></CmdData>";
        
        public CommandDataInfo(ArchestraAttribute attribute) : base(attribute, DefaultValue)
        {
            if (attribute.Name != AttributeName)
                throw new InvalidOperationException($"Attribute name is not expected value' {AttributeName}'");
        }

        public override void Add(XElement element, string parentName = null)
        {
            var name = parentName ?? "BooleanLabel";
            base.Add(element, name);
        }
        
        public override void Replace(XElement element, string parentName = null)
        {
            var name = parentName ?? "BooleanLabel";
            base.Add(element, name);
        }
    }
}