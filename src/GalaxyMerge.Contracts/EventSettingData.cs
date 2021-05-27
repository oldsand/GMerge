using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    public class EventSettingData
    {
        public Operation Operation { get; set; }
        public OperationType OperationType { get; set; }
        public bool IsArchiveTrigger { get; set; }
    }
}