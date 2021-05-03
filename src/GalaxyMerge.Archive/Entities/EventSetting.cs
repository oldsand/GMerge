using GalaxyMerge.Archive.Enum;

namespace GalaxyMerge.Archive.Entities
{
    public class EventSetting
    {
        protected EventSetting()
        {
        }
        
        public EventSetting(int eventId, string eventName, EventType eventType)
        {
            EventId = eventId;
            EventName = eventName;
            EventType = eventType;
        }
        
        public int EventId { get; private set; }
        public string EventName { get;  private set; }
        public EventType EventType { get; private set; }
    }
}