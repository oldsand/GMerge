namespace GalaxyMerge.Archive.Entities
{
    public class ArchiveEvent
    {
        protected ArchiveEvent()
        {
        }
        
        public ArchiveEvent(int eventId, string eventName, bool creationEvent)
        {
            EventId = eventId;
            EventName = eventName;
            CreationEvent = creationEvent;
        }
        
        public int EventId { get; private set; }
        public string EventName { get;  private set; }
        public bool CreationEvent { get; private set; }
    }
}