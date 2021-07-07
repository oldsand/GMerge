using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Primitives;

namespace GalaxyMerge.Archiving.Abstractions
{
    public interface IArchiveConfiguration
    {
        Archive GenerateArchive();
        ArchiveConfiguration HasEvent(Operation operation, bool isArchiveEvent);
        ArchiveConfiguration HasInclusion(Template template, InclusionOption option, bool includeInstances);
    }
}