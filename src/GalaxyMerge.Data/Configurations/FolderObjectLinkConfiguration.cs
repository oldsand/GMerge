using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Data.Configurations
{
    public class FolderObjectLinkConfiguration : IEntityTypeConfiguration<FolderObjectLink>
    {
        public void Configure(EntityTypeBuilder<FolderObjectLink> builder)
        {
            builder.ToView("folder_gobject_link").HasKey(x => new {x.FolderId, x.ObjectId});
            
            builder.Property(x => x.FolderId).HasColumnName("folder_id");
            builder.Property(x => x.ObjectId).HasColumnName("gobject_id");
        }
    }
}