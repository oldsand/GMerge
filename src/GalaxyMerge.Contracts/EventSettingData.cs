using GalaxyMerge.Core;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Contracts
{
    public class EventSettingData
    {
        public int OperationId { get; set; }
        public string OperationName { get; set; }
        public OperationType OperationType { get; set; }
        public bool IsArchiveTrigger { get; set; }
        public Operation Operation => Enumeration.FromId<Operation>(OperationId);
    }
}