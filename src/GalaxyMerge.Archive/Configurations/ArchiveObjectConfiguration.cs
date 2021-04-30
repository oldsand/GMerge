using GalaxyMerge.Archive.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class ArchiveObjectConfiguration : IEntityTypeConfiguration<ArchiveObject>
    {
        public void Configure(EntityTypeBuilder<ArchiveObject> builder)
        {
            builder.ToTable("Object").HasKey(x => x.ObjectId);
            builder.Property(x => x.TagName).IsRequired();
            builder.Property(x => x.BaseType).IsRequired();
            builder.Property(x => x.Version).IsRequired();
            builder.HasMany(x => x.Entries).WithOne(x => x.ArchiveObject).HasForeignKey(x => x.ObjectId);
        }
    }
}