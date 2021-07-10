using GCommon.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Data.Configurations
{
    public class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            //Configuring as view because EF core will treat this table as read-only, which we want.
            builder.ToView("folder").HasKey(x => x.FolderId);
            
            builder.Property(x => x.FolderId).HasColumnName("folder_id");
            builder.Property(x => x.FolderType).HasColumnName("folder_type");
            builder.Property(x => x.FolderName).HasColumnName("folder_name");
            builder.Property(x => x.ParentFolderId).HasColumnName("parent_folder_id");
            builder.Property(x => x.Depth).HasColumnName("depth");
            builder.Property(x => x.HasObjects).HasColumnName("has_objects");
            builder.Property(x => x.HasFolders).HasColumnName("has_folders");

            builder.Ignore(x => x.Objects);

            builder.HasMany(x => x.Folders).WithOne(x => x.ParentFolder).HasForeignKey(x => x.ParentFolderId);
            builder.HasMany(x => x.FolderObjectLinks).WithOne(x => x.Folder).HasForeignKey(x => x.FolderId);
        }
    }
}