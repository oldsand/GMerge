using GalaxyMerge.Core;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archive.Entities
{
    public class EventSetting
    {
        private EventSetting()
        {
        }
        
        public EventSetting(Operation operation)
        {
            OperationId = operation.Id;
            OperationType = OperationType.FromOperation(operation);
            IsArchiveTrigger = false;
        }
        
        public EventSetting(Operation operation, bool isArchiveTrigger)
        {
            OperationId = operation.Id;
            OperationType = OperationType.FromOperation(operation);
            IsArchiveTrigger = isArchiveTrigger;
        }
        
        public int OperationId { get; private set; }
        public Operation Operation => Enumeration.FromId<Operation>(OperationId);
        public OperationType OperationType { get; private set; }
        public bool IsArchiveTrigger { get; private set; }
        
        public void SetArchiveTrigger(bool isArchiveTrigger)
        {
            IsArchiveTrigger = isArchiveTrigger;
        }
    }
}