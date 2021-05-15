using GalaxyMerge.Common.Primitives;

namespace GalaxyMerge.Contracts
{
    public class NamedScriptData
    {
        public string Name { get; set; }
        public int DeadBand { get; set; }
        public string Expression { get; set; }
        public ScriptTrigger Trigger { get; set; }
        public int TriggerPeriod { get; set; }
        public string Text { get; set; }
    }
}