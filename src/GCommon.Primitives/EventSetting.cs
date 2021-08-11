using GCommon.Primitives.Enumerations;

namespace GCommon.Primitives
{
    public class EventSetting
    {
        private EventSetting()
        {
        }
        
        public EventSetting(Operation operation, bool isArchiveEvent = false)
        {
            Operation = operation;
            OperationType = OperationType.FromOperation(operation);
            IsArchiveEvent = isArchiveEvent;
        }

        public int EventId { get; private set; }
        public int ArchiveId { get; private set; }
        public Archive Archive { get; private set; }
        public Operation Operation { get; private set; }
        public OperationType OperationType { get; private set; }
        public bool IsArchiveEvent { get; set; }
    }
}