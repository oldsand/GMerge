using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;

namespace GalaxyMerge.Common.Abstractions
{
    public interface INamedScript : IXmlConvertible<INamedScript>
    {
        string Name { get; }
        int DeadBand { get; }
        string Expression { get; }
        ScriptTrigger Trigger { get; }
        int TriggerPeriod { get; }
        string Text { get; }
    }
}