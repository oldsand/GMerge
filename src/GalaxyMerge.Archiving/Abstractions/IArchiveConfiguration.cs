using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Abstractions
{
    public interface IArchiveConfiguration
    {
        Archive GenerateArchive();
        ArchiveConfiguration ConfigureEvent(Operation operation, bool isArchiveEvent);
        ArchiveConfiguration ConfigureInclusion(Template template, InclusionOption option, bool includeInstances);
    }
}