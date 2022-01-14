using Ardalis.SmartEnum;

namespace GCommon.Core.Enumerations
{
    public class ExtensionType : SmartEnum<ExtensionType>
    {
        public ExtensionType(string name, int value) : base(name, value)
        {
        }

        public static readonly ExtensionType Object = new ExtensionType("Object", 1);
        public static readonly ExtensionType Attribute = new ExtensionType("Attribute", 2);
    }
}