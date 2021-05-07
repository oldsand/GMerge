using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;

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
            OperationName = operation.Name;
            OperationType = OperationType.FromOperation(operation);
            IsArchiveTrigger = false;
        }
        
        public EventSetting(Operation operation, bool isArchiveTrigger)
        {
            OperationId = operation.Id;
            OperationName = operation.Name;
            OperationType = OperationType.FromOperation(operation);
            IsArchiveTrigger = isArchiveTrigger;
        }

        public int EventId { get; private set; }
        public int OperationId { get; private set; }
        public string OperationName { get; private set; }
        public OperationType OperationType { get; private set; }
        public bool IsArchiveTrigger { get; private set; }
        public Operation Operation => Enumeration.FromId<Operation>(OperationId);

        public void SetArchiveTrigger(bool isArchiveTrigger)
        {
            IsArchiveTrigger = isArchiveTrigger;
        }
    }
}