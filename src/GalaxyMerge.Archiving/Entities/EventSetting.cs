using GalaxyMerge.Core;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Entities
{
    public class EventSetting
    {
        private EventSetting()
        {
        }
        
        public EventSetting(Operation operation, bool isArchiveEvent = false)
        {
            OperationId = operation.Id;
            IsArchiveEvent = isArchiveEvent;
        }

        public int EventId { get; private set; }
        public int OperationId { get; private set; }
        public Operation Operation => Enumeration.FromId<Operation>(OperationId);
        public OperationType OperationType => OperationType.FromOperation(Operation);
        public bool IsArchiveEvent { get; set; }
    }
}