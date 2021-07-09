using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archiving.Configurations
{
    public class ArchiveObjectConfiguration : IEntityTypeConfiguration<ArchiveObject>
    {
        public void Configure(EntityTypeBuilder<ArchiveObject> builder)
        {
            builder.ToTable(nameof(ArchiveObject)).HasKey(x => x.ObjectId);
            
            builder.Property(x => x.TagName).IsRequired();
            builder.Property(x => x.Template).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<Template>(x));
            builder.Property(x => x.Version).IsRequired();
            builder.Property(x => x.AddedOn).IsRequired();
            builder.Property(x => x.ModifiedOn).IsRequired();

            builder.HasMany(x => x.Entries).WithOne(x => x.ArchiveObject).HasForeignKey(x => x.ObjectId);
            builder.HasMany(x => x.QueuedItems).WithOne(x => x.ArchiveObject).HasForeignKey(x => x.ObjectId);
        }
    }
}