using System;

namespace GCommon.Data.Abstractions
{
    public interface IGalaxyDataProvider : IDisposable
    {
        IObjectRepository Objects { get; }
        IDefinitionRepository Definitions { get; }
        IFolderRepository Folders { get; }
        IChangeLogRepository ChangeLogs { get; }
        IUserRepository Users { get; }
        ILookupRepository Lookup { get; }
    }
}