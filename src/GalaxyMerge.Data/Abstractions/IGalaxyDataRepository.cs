using System;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IGalaxyDataRepository : IDisposable
    {
        IObjectRepository Objects { get; }
        IDefinitionRepository Definitions { get; }
        IFolderRepository Folders { get; }
        IChangeLogRepository ChangeLogs { get; }
        IUserRepository Users { get; }
        ILookupRepository Lookup { get; }
    }
}