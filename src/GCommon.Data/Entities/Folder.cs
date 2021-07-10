// EF Core entity class. Only EF should be instantiating and setting properties.
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

using System.Collections.Generic;
using System.Linq;

namespace GCommon.Data.Entities
{
    public class Folder
    {
        private Folder()
        {
            Folders = new List<Folder>();
            FolderObjectLinks = new List<FolderObjectLink>();
        }

        public int FolderId { get; private set; }
        public short FolderType { get; private set; }
        public string FolderName { get; private set; }
        public int ParentFolderId { get; private set; }
        public int Depth { get; private set; }
        public bool HasObjects { get; private set; }
        public bool HasFolders { get; private set; }
        public Folder ParentFolder { get; set; }
        public IEnumerable<Folder> Folders { get; private set; }
        internal IEnumerable<FolderObjectLink> FolderObjectLinks { get; private set; }
        public IEnumerable<GObject> Objects => FolderObjectLinks.Select(x => x.GObject);
    }
}