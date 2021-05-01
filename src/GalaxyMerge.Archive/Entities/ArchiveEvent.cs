namespace GalaxyMerge.Archive.Entities
{
    public class ArchiveEvent
    {
        protected ArchiveEvent()
        {
        }
        
        public ArchiveEvent(int eventId, string eventName, bool isCreationEvent)
        {
            EventId = eventId;
            EventName = eventName;
            IsCreationEvent = isCreationEvent;
        }
        
        public int EventId { get; private set; }
        public string EventName { get;  private set; }
        public bool IsCreationEvent { get; private set; }
    }
}