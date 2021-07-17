namespace GCommon.Data.Entities
{
    public class FolderObjectLink
    {
        private FolderObjectLink()
        {
        }
        
        public int FolderId { get; private set; }
        public int ObjectId { get; private set; }
        public Folder Folder { get; private set; }
        public GalaxyObject GalaxyObject { get; private set; }
    }
}