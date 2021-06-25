using GalaxyMerge.Client.Wrappers;
using Prism.Events;
using Prism.Services.Dialogs;

namespace GalaxyMerge.Client.Events
{
    public class NewResourceCompleteEvent : PubSubEvent<NewResourceCompleteEventArgs>
    {
    }

    public class NewResourceCompleteEventArgs
    {
        public NewResourceCompleteEventArgs(ButtonResult result, ResourceEntryWrapper entry)
        {
            Result = result;
            ResourceEntry = entry;
        }
        public ButtonResult Result { get; set; }
        public ResourceEntryWrapper ResourceEntry { get; set; }
    }
}