using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class ArchiveObjectConfiguration : IEntityTypeConfiguration<ArchiveObject>
    {
        public void Configure(EntityTypeBuilder<ArchiveObject> builder)
        {
            builder.ToTable("ArchiveObject").HasKey(x => x.ObjectId);
            
            builder.Property(x => x.TagName).IsRequired();
            builder.Property(x => x.Template).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<Template>(x));
            builder.Property(x => x.Version).IsRequired();
            builder.Property(x => x.AddedOn).IsRequired();
            builder.Property(x => x.ModifiedOn).IsRequired();
            
            builder.HasMany(x => x.Entries).WithOne(x => x.ArchiveObject).HasForeignKey(x => x.ObjectId);
        }
    }
}