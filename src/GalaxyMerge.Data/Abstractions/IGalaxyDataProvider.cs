using System;

namespace GalaxyMerge.Data.Abstractions
{
    public interface IGalaxyDataProvider : IDisposable
    {
        IObjectReadOnlyRepository ObjectsReadOnly { get; }
        IDefinitionReadOnlyRepository DefinitionsReadOnly { get; }
        IFolderReadOnlyRepository FoldersReadOnly { get; }
        IChangeLogReadOnlyRepository ChangeLogsReadOnly { get; }
        IUserReadOnlyRepository UsersReadOnly { get; }
        ILookupRepository Lookup { get; }
    }
}