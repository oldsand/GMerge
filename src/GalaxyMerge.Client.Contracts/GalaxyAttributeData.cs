using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    public class GalaxyAttributeData
    {
        public string Name { get; set; }
        public DataType DataType { get; set; }
        public AttributeCategory Category { get; set; }
        public SecurityClassification Security { get; set; }
        public LockType Locked { get; set; }
        public object Value { get; set; }
        public int ArrayCount { get; set; }
    }
}